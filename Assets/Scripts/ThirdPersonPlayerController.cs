using System;
using System.Collections.Generic;
using Annotations;
using UnityEngine;

[Feature(FeatureTag.PlayerController)]
[Feature(FeatureTag.PlayerInputHandler)]
public class ThirdPersonPlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private CapsuleCollider _playerCollider;
    [SerializeField] private Transform _feetTransform;

    private Rigidbody _rigidbody;
    private MainPlayerControl _control;

    private Vector3 _targetVelocity;
    private Quaternion _targetRotation;

    private Vector3 _animationVelocity;

    private Animator _animator;

    private bool _useGravity;


    public bool IsInLocomotionAnimation { get; set; }

    public PlayerControllerState State { get; private set; } = PlayerControllerState.Locomotion;

    public PlayerControllerState TickCurrentState(float deltaTime)
    {
        switch (State)
        {
            case PlayerControllerState.Locomotion:
                return TickLocomotion(deltaTime);
            case PlayerControllerState.Rolling:
                return TickRolling(deltaTime);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private PlayerControllerState TickRolling(float deltaTime)
    {
        if (IsInLocomotionAnimation)
        {
            return PlayerControllerState.Locomotion;
        }
        else
        {
            return PlayerControllerState.Rolling;
        }
    }

    private PlayerControllerState EnterRolling()
    {
        _animator.SetTrigger("Roll Trigger");
        return PlayerControllerState.Rolling;
    }
    
    private PlayerControllerState TickLocomotion(float deltaTime)
    {
        var axisInput = _control.Standard.Locomotion.ReadValue<Vector2>();
        var fwd = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        var right = Vector3.Cross(Vector3.up, fwd).normalized;

        var velocity = Vector3.zero;
        var rotation = _playerTransform.rotation;

        if (axisInput.magnitude > 0.2f)
        {
            axisInput = axisInput.normalized * Mathf.Min(1.0f, axisInput.magnitude);
            velocity = (axisInput.x * right + axisInput.y * fwd) * _maxSpeed;
            rotation = Quaternion.LookRotation(velocity.normalized);
        }

        _targetVelocity = velocity;
        _targetRotation = rotation;

        UpdatePlayerRotation();
        
        if (_control.Standard.Roll.ReadValue<float>() > 0.5f)
        {
            _animator.SetTrigger("Roll Trigger");
        }

        if (!IsInLocomotionAnimation)
        {
            return PlayerControllerState.Rolling;
        }

        return PlayerControllerState.Locomotion;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _control = MainInputHandler.Instance.Control;
        _rigidbody = _playerTransform.GetComponent<Rigidbody>();
        _targetRotation = Quaternion.identity;
        _locomotionClipInfo = new List<AnimatorClipInfo>(4);
    }

    private void Update()
    {
        State = TickCurrentState(Time.deltaTime);
    }

    private List<AnimatorClipInfo> _locomotionClipInfo;

    private void FixedUpdate()
    {
        UpdateHorizontalVelocity();
        UpdateGravity();

        if (State == PlayerControllerState.Locomotion)
        {
            var actualVelocity3D = _rigidbody.velocity;
            var actualVelocity = new Vector2(actualVelocity3D.x, actualVelocity3D.z);
            var actualSpeed = actualVelocity.magnitude;
            var normalizedSpeed = Mathf.Clamp01(Mathf.InverseLerp(0.0f, _maxSpeed, actualSpeed));
            _animator.SetFloat("Speed", normalizedSpeed);
            _animator.GetCurrentAnimatorClipInfo(0, _locomotionClipInfo);
            var averageClipSpeed = 0.0f;
            foreach (var clipInfo in _locomotionClipInfo)
            {
                averageClipSpeed += clipInfo.weight * clipInfo.clip.averageSpeed.z;
            }

            if (averageClipSpeed <= 0.1f)
            {
                _animator.SetFloat("Locomotion Multiplier", 1.0f);
            }
            else
            {
                var multiplier = actualSpeed / averageClipSpeed;
                _animator.SetFloat("Locomotion Multiplier", multiplier);
            }
        }
        else
        {
            _targetVelocity = Vector3.zero;
        }
    }

    private void SetYSpeed(float ySpeed)
    {
        var velocity = _rigidbody.velocity;
        velocity.y = ySpeed;
        _rigidbody.velocity = velocity;
    }

    private void LateUpdate()
    {
        // UpdateGravity();
    }

    private enum VerticalMotionMode
    {
        None,
        Gravity,
        Penetration
    }

    private void UpdateGravity()
    {
        var rayStart = _playerCollider.GetBottomPoint();
        var expectedRayLength = Mathf.Max(0, rayStart.y - _feetTransform.position.y);
        var rayHitsGround = Physics.Raycast(new Ray(rayStart, Vector3.down), out var hit, expectedRayLength + 1f);
        var motionMode = VerticalMotionMode.None;
        if (rayHitsGround)
        {
            if (hit.distance < expectedRayLength - 0.05f)
            {
                motionMode = VerticalMotionMode.Penetration;
            }
            else if (hit.distance > expectedRayLength + 0.05f)
            {
                motionMode = VerticalMotionMode.Gravity;
            }
        }
        else
        {
            motionMode = VerticalMotionMode.Gravity;
        }

        switch (motionMode)
        {
            case VerticalMotionMode.None:
            {
                SetYSpeed(0.0f);
                break;
            }
            case VerticalMotionMode.Gravity:
            {
                SetYSpeed(-5.0f);
                break;
            }
            case VerticalMotionMode.Penetration:
            {
                SetYSpeed(0.0f);
                _rigidbody.MovePosition(_rigidbody.position + Vector3.up * (expectedRayLength - hit.distance));
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        // if ()
        // {
        //     if (hit.distance < expectedRayLength - 0.1f)
        //     {
        //         _useGravity = false;
        //     }
        //
        // }
        // else
        // {
        //     _useGravity = true;
        // }
    }

    private void UpdateHorizontalVelocity()
    {
        var velocity = _rigidbody.velocity;
        var targetVelocity = new Vector3(_targetVelocity.x, velocity.y, _targetVelocity.z);
        velocity = Vector3.Lerp(velocity, targetVelocity, 0.5f);
        _rigidbody.velocity = velocity + _animationVelocity;
    }

    private void UpdatePlayerRotation()
    {
        var rotation = _playerTransform.rotation;
        rotation = Quaternion.Slerp(rotation, _targetRotation, 15f * Time.deltaTime);
        _playerTransform.rotation = rotation;
    }

    private void OnAnimatorMove()
    {
        if (State == PlayerControllerState.Locomotion)
        {
            return;
        }
        else
        {
            var deltaTime = Time.deltaTime;
            var deltaPosition = _animator.deltaPosition;
            var velocity = deltaPosition / deltaTime;
            _animationVelocity = velocity;
        }
        
    }
}