using System;
using Cinemachine;
using UnityEngine;

namespace CameraSystem
{
    public class CharacterCameraSetup : MonoBehaviour
    {
        [Header("Shared")]
        [SerializeField] private CinemachineTargetGroup _targetGroup;

        [Header("Free Look")]
        [SerializeField] private CinemachineVirtualCamera _freeLookCamera;
        [SerializeField] private FreeOrbitalCameraPivot _freeLookPivot;

        [Header("Lock On")]
        [SerializeField] private CinemachineVirtualCamera _lockOnCamera;
        [SerializeField] private LockOnCameraPivot _lockOnPivot;

        public CharacterCameraMode CurrentMode { get; private set; }
        private Transform _lockOnTarget;

        private void Awake()
        {
            CurrentMode = CharacterCameraMode.FreeMovement;
        }

        public void BeginLockOn(Transform target)
        {
            Debug.Assert(target != null);

            if (CurrentMode == CharacterCameraMode.TargetLockOn)
            {
                throw new Exception(
                    $"The character's camera is already in lock on mode, with target '{_lockOnTarget.gameObject}'");
            }

            CurrentMode = CharacterCameraMode.TargetLockOn;
            _lockOnTarget = target;
            
            _freeLookCamera.Priority = -1;

            _lockOnCamera.Priority = 10;
            _lockOnPivot.TargetTransform = target;

            _targetGroup.m_Targets[1].target = target;
        }

        public void EndLockOn()
        {
            Debug.Assert(CurrentMode == CharacterCameraMode.TargetLockOn);

            CurrentMode = CharacterCameraMode.FreeMovement;
            _lockOnTarget = null;
            
            _freeLookCamera.Priority = 10;
            _freeLookPivot.SyncWith(_lockOnPivot.PivotTransform);
            _lockOnCamera.Priority = -1;
            
            _targetGroup.m_Targets[1].target = null;
        }

        private void Update()
        {
            if (CurrentMode == CharacterCameraMode.TargetLockOn && _lockOnTarget == null)
            {
                EndLockOn();
            }
        }
    }
}