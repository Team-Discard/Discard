using System.Collections.Generic;
using Animancer;
using UnityEngine;
using Unstable.Actions.GreatSwordSlash;
using Unstable.Utils;
using WeaponSystem;

namespace Unstable.Entities
{
    public class PlayerMasterController :
        MonoBehaviour,
        ITicker
    {
        [SerializeField] private PlayerPawn _playerPawn;
        [SerializeField] private PlayerLocomotionController _locomotionController;
        [SerializeField] private GreatSwordSlashAction _chargeActionPrefab;
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private Camera _controlCamera;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _noWeaponLocomotionAnimations;
        
        [SerializeField] private float _maxSpeed;

        private ActionExecutor _actionExecutor;
        private List<ActionEffects> _actionEffects;

        private PawnAnimationHandler _animationHandler;
        
        public List<ActionEffects> CurrentActionEffects => _actionEffects;
        
        
        #region Triggers

        #endregion

        private void Awake()
        {
            _actionExecutor = new ActionExecutor();
            _actionEffects = new List<ActionEffects>();
            _animationHandler = new PawnAnimationHandler(_playerPawn, _animancer, _noWeaponLocomotionAnimations);
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

            var forwardSpeed = _playerPawn.CalculateForwardSpeed();
            var normalizedSpeed = Mathf.InverseLerp(0.0f, _maxSpeed, forwardSpeed);
            
            _animationHandler.SetNormalizedSpeed(normalizedSpeed);
            
            HandleActionTriggers(_actionExecutor);
            
            _animationHandler.Tick(deltaTime);
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

        private IAction CreateChargeAction()
        {
            var charge = Instantiate(_chargeActionPrefab, _playerPawn.transform);
            charge.Init(_playerPawn);
            return charge;
        }
    }
}