using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using UnityEngine;
using Unstable.Actions.GreatSwordSlash;
using Unstable.Utils;
using Uxt;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Entities
{
    [SelectionBase]
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
        [SerializeField] private WeaponTriggers _weaponTriggers;

        [SerializeField] private Transform _swordHandleBottom;
        [SerializeField] private Transform _swordHandleTop;

        [SerializeField] private PlayerMovementSmoother _smoother;
        
        [SerializeField] private float _maxSpeed;

        private ActionExecutor _actionExecutor;
        private List<ActionEffects> _actionEffects;

        private PawnAnimationHandler _animationHandler;

        private Sword _activeSwordEquipped;

        public List<ActionEffects> CurrentActionEffects => _actionEffects;


        #region Triggers

        #endregion

        private void Awake()
        {
            _actionExecutor = new ActionExecutor();
            _actionEffects = new List<ActionEffects>();
            _animationHandler = new PawnAnimationHandler(_playerPawn, _animancer, _noWeaponLocomotionAnimations);
            _weaponTriggers = new WeaponTriggers();
            _activeSwordEquipped = null;
        }

        public void Tick(float deltaTime)
        {
            _inputHandler.UpdateInput(out var inputDirection);
            var controlDirection = _controlCamera.transform.forward.ConvertXz2Xy();
            var translationFrame = new TranslationFrame();
            var rotationFrame = _playerPawn.GetRotationFrame();
            _actionExecutor.Execute(deltaTime, _actionEffects);

            if (!AnyActionDisablesFreeMovement(_actionEffects))
            {
                _locomotionController.ApplyDirectionalMovement(
                    Time.deltaTime, inputDirection, controlDirection, _maxSpeed,
                    ref translationFrame, ref rotationFrame);
            }

            _locomotionController.ApplyActionEffects(deltaTime, _actionEffects, ref translationFrame);
            _playerPawn.SetTranslationFrame(translationFrame);
            _playerPawn.SetRotationFrame(rotationFrame);

            var forwardSpeed = _playerPawn.CalculateForwardSpeed();
            var normalizedSpeed = Mathf.InverseLerp(0.0f, _maxSpeed, forwardSpeed);

            _animationHandler.SetNormalizedSpeed(normalizedSpeed);
            _animationHandler.SetAbsoluteSpeed(forwardSpeed);

            HandleActionTriggers(_actionExecutor);

            if (_weaponTriggers.UnEquipTrigger.Consume(out var _))
            {
                _animationHandler.SetLocomotionAnimations(_noWeaponLocomotionAnimations);

                if (_activeSwordEquipped != null)
                {
                    UnEquipActiveSword();
                }
            }

            if (_weaponTriggers.EquipTrigger.Consume(out var equipDesc))
            {
                if (equipDesc.Sword is { } equipSword)
                {
                    _animationHandler.SetLocomotionAnimations(equipSword.LocomotionAnimations);
                    var swordPrefab = equipSword.SwordPrefab;
                    if (_activeSwordEquipped == null && swordPrefab != null)
                    {
                        EquipSword(Instantiate(swordPrefab, transform.position, Quaternion.identity));
                    }
                }
            }
            
            _smoother.Tick(deltaTime);

            if (_activeSwordEquipped != null)
            {
                MatchSwordToHandPosition();
            }
            
            _animationHandler.Tick(deltaTime);
        }

        private void FixedUpdate()
        {
            if (_activeSwordEquipped != null)
            {
                MatchSwordToHandPosition();
            }
        }

        private void MatchSwordToHandPosition()
        {
            var swordTransform = _activeSwordEquipped.transform;
            var swordCenterTransform = _activeSwordEquipped.Center;
            var target = IkUtility.MatchStickWithHandlePoints(_swordHandleBottom, _swordHandleTop);
            var swordTransformData = IkUtility.MoveParentToMatchChild(swordTransform, swordCenterTransform, target);
            swordTransformData.ApplyTo(swordTransform);
        }

        private void UnEquipActiveSword()
        {
            Destroy(_activeSwordEquipped.gameObject);
        }

        private void EquipSword(Sword swordInstance)
        {
            if (_activeSwordEquipped != null)
            {
                Debug.LogError(
                    $"[Weapon Equip System] Player could not equip another sword" +
                    $" without first un-equipping the old one ({_activeSwordEquipped.gameObject.name})");
                return;
            }

            _activeSwordEquipped = swordInstance;
        }

        private static bool AnyActionDisablesFreeMovement(List<ActionEffects> actionEffects)
        {
            return actionEffects.Any(effect => !effect.FreeMovementEnabled);
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
            charge.Init(_playerPawn, _animationHandler, _weaponTriggers);
            return charge;
        }
    }
}