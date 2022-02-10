using System;
using UnityEngine;

namespace Unstable.Entities
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private MainPlayerControl _control;

        private void Awake()
        {
            _control = new MainPlayerControl();
            _control.Enable();
            RegisterInput();
        }

        private void OnDestroy()
        {
            UnregisterInput();
        }

        public void UpdateInput(
            out Vector2 inputDirection)
        {
            inputDirection = _control.Standard.Locomotion.ReadValue<Vector2>();
        }

        private void RegisterInput()
        {
            var standard = _control.Standard;
        }

        private void UnregisterInput()
        {
        }
    }
}