using System;
using System.Collections.Generic;
using UnityEngine;
using Unstable.PlayerActions.Charge;
using Unstable.Utils;

namespace Unstable.Entities
{
    public class PlayerMasterController :
        MonoBehaviour,
        ITicker
    {
        [SerializeField] private PlayerPawn _playerPawn;
        [SerializeField] private PlayerLocomotionController _locomotionController;
        [SerializeField] private ChargeAction _chargeActionPrefab;
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private Camera _controlCamera;
        [SerializeField] private GameLoop _gameLoop;

        [SerializeField] private float _maxSpeed;

        private ActionExecutor _actionExecutor;
        private List<ActionEffects> _actionEffects;

        public List<ActionEffects> CurrentActionEffects => _actionEffects; 
        
        #region Triggers

        #endregion

        private void Awake()
        {
            _actionExecutor = new ActionExecutor();
            _actionEffects = new List<ActionEffects>();
        }

        public void Tick(float deltaTime)
        {
            _inputHandler.UpdateInput(out var inputDirection);
            var controlDirection = _controlCamera.transform.forward.ConvertXz2Xy();
            var translationFrame = new TranslationFrame();
            var rotationFrame = _playerPawn.GetRotationFrame();
            _actionExecutor.Execute(deltaTime, _actionEffects);

            _locomotionController.ApplyDirectionalMovement(
                Time.deltaTime, inputDirection, controlDirection, _maxSpeed,
                ref translationFrame, ref rotationFrame);
            _locomotionController.ApplyEffects(deltaTime, _actionEffects, ref translationFrame);
            _playerPawn.SetTranslationFrame(translationFrame);
            _playerPawn.SetRotationFrame(rotationFrame);

            HandleActionTriggers(_actionExecutor);
        }

        private void HandleActionTriggers(ActionExecutor actionExecutor)
        {
            if (actionExecutor.HasPendingOrActiveActions)
            {
                return;
            }

            if (MainInputHandler.Instance.Control.Standard.Roll.WasPerformedThisFrame())
            {
                var charge = CreateChargeAction();
                actionExecutor.AddAction(charge);
            }
        }

        private ChargeAction CreateChargeAction()
        {
            var charge = Instantiate(_chargeActionPrefab, _playerPawn.transform);
            charge.Init(_playerPawn);
            return charge;
        }
    }
}