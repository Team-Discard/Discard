using System;
using UnityEngine;

[Obsolete]
public class LegacyPlayerController : MonoBehaviour
{
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private CapsuleCollider _playerCollider;
    private Vector2 _moveDirection;

    private Vector2 _targetVelocity;
    private MainPlayerControl _controls;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _controls = MainInputHandler.Instance.Control;
    }

    private void Update()
    {
        ReadLocomotionInput();
    }

    private void ReadLocomotionInput()
    {
        _moveDirection = _controls.Standard.Locomotion.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        UpdateTargetVelocity();
        UpdateHorizontalVelocity();
        UpdateGravity();
    }

    private void UpdateGravity()
    {
        var rayStart = _playerCollider.GetBottomPoint();
        var expectedRayLength = Mathf.Max(0, rayStart.y - _feetTransform.position.y);
        if (Physics.Raycast(new Ray(rayStart, Vector3.down), out var hit, expectedRayLength))
        {
            if (hit.distance < expectedRayLength)
            {
                _rigidbody.MovePosition(_rigidbody.position + Vector3.up * (expectedRayLength - hit.distance));
            }

            _rigidbody.useGravity = false;
        }
        else
        {
            _rigidbody.useGravity = true;
        }
    }

    private void UpdateHorizontalVelocity()
    {
        var velocity = _rigidbody.velocity;
        var targetVelocity = new Vector3(_targetVelocity.x, velocity.y, _targetVelocity.y);
        velocity = Vector3.Lerp(velocity, targetVelocity, 0.5f);
        _rigidbody.velocity = velocity;
    }

    private void UpdateTargetVelocity()
    {
        var magnitude = _moveDirection.magnitude;
        if (magnitude < 0.2f)
        {
            _targetVelocity = Vector2.zero;
        }
        else
        {
            _targetVelocity = _moveDirection * _maxMoveSpeed;
        }
    }
}