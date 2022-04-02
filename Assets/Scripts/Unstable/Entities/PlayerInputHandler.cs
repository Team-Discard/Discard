using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Unstable.Entities
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private MainPlayerControl _control;

        public Action onSouthButton;
        public Action onEastButton;
        public Action onNorthButton;
        public Action onWestButton;
        public Action onToggleLockOn;

        private void Awake()
        {
            onSouthButton = null;
            onEastButton = null;
            onNorthButton = null;
            onWestButton = null;
            onToggleLockOn = null;
            
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

        private void OnSouthButton(InputAction.CallbackContext callbackContext)
        {
            onSouthButton?.Invoke();
        }

        private void OnEastButton(InputAction.CallbackContext callbackContext)
        {
            onEastButton?.Invoke();
        }

        private void OnNorthButton(InputAction.CallbackContext callbackContext)
        {
            onNorthButton?.Invoke();
        }

        private void OnWestButton(InputAction.CallbackContext callbackContext)
        {
            onWestButton?.Invoke();
        }

        private void OnToggleLockOn(InputAction.CallbackContext callbackContext)
        {
            onToggleLockOn?.Invoke();
        }

        private void RegisterInput()
        {
            _control.Standard.CardSouth.performed += OnSouthButton;
            _control.Standard.CardEast.performed += OnEastButton;
            _control.Standard.CardNorth.performed += OnNorthButton;
            _control.Standard.CardWest.performed += OnWestButton;
            _control.Standard.ToggleLockOn.performed += OnToggleLockOn;
        }

        private void UnregisterInput()
        {
            _control.Standard.CardSouth.performed -= OnSouthButton;
            _control.Standard.CardEast.performed -= OnEastButton;
            _control.Standard.CardNorth.performed -= OnNorthButton;
            _control.Standard.CardWest.performed -= OnWestButton;
            _control.Standard.ToggleLockOn.performed -= OnToggleLockOn;
        }
    }
}