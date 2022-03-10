using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using CardSystem;
using UnityEngine;
using Unstable.Actions;
using Unstable.Actions.GreatSwordSlash;
using Unstable.PlayerActions.Charge;
using Unstable.Utils;
using Uxt;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Entities
{
    [SelectionBase]
    public class PlayerMasterController :
        MonoBehaviour,
        ITicker,
        IActionVisitor
    {
        [SerializeField] private PlayerPawn _playerPawn;
        [SerializeField] private LocomotionController _locomotionController;
        [SerializeField] private GreatSwordSlashAction _chargeActionPrefab;
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private Camera _controlCamera;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _noWeaponLocomotionAnimations;

        [SerializeField] private Transform _swordHandleBottom;
        [SerializeField] private Transform _swordHandleTop;

        [SerializeField] private PlayerMovementSmoother _smoother;
        
        [SerializeField] private float _maxSpeed;
        
        [SerializeField] private List<Card> _cards;
        private TemporaryCardTextUI _cardUi;

        private WeaponTriggers _weaponTriggers;
        private ActionExecutor _actionExecutor;
        private List<ActionEffects> _actionEffects;

        private PawnAnimationHandler _animationHandler;

        private Sword _swordEquipped;

        public List<ActionEffects> CurrentActionEffects => _actionEffects;

        #region Triggers

        #endregion

        private void Awake()
        {
            _actionExecutor = new ActionExecutor();
            _actionEffects = new List<ActionEffects>();
            _animationHandler = new PawnAnimationHandler(_playerPawn, _animancer, _noWeaponLocomotionAnimations);
            _weaponTriggers = new WeaponTriggers();
            _swordEquipped = null;
            _cardUi = new TemporaryCardTextUI(_cards);
        }

        private void Start()
        {
            _inputHandler.onSouthButton += UseSouthCard;
            _inputHandler.onEastButton += UseEastCard;
            _inputHandler.onNorthButton += UseNorthCard;
            _inputHandler.onWestButton += UseWestCard;
        }

        public void Tick(float deltaTime)
        {
            _inputHandler.UpdateInput(out var inputDirection);
            var controlDirection = _controlCamera.transform.forward.ConvertXz2Xy();
            var translationFrame = new TranslationFrame();
            var rotationFrame = _playerPawn.GetRotationFrame().PrepareNextFrame();
            _actionExecutor.Execute(deltaTime, _actionEffects);

            if (!AnyActionDisablesFreeMovement(_actionEffects))
            {
                _locomotionController.ApplyDirectionalMovement(
                    Time.deltaTime, inputDirection, controlDirection, _maxSpeed,
                    ref translationFrame, ref rotationFrame);
            }

            _locomotionController.ApplyGravity(deltaTime, ref translationFrame);
            _locomotionController.ApplyActionEffects(deltaTime, _actionEffects, ref translationFrame);
            _playerPawn.SetTranslationFrame(translationFrame);
            _playerPawn.SetRotationFrame(rotationFrame);

            var forwardSpeed = _playerPawn.CalculateForwardSpeed();

            _animationHandler.SetAbsoluteSpeed(forwardSpeed);

            HandleActionTriggers(_actionExecutor);

            if (_weaponTriggers.UnEquipAllWeapon.Consume(out var _, out var onUnEquipSucceed))
            {
                _animationHandler.SetLocomotionAnimations(_noWeaponLocomotionAnimations);

                if (_swordEquipped != null)
                {
                    UnEquipActiveSword();
                }

                onUnEquipSucceed?.Invoke(true);
            }

            if (_weaponTriggers.EquipSword.Consume(out var equipSword, out var onEquipSwordSucceed))
            {
                _animationHandler.SetLocomotionAnimations(equipSword.LocomotionAnimations);
                var swordPrefab = equipSword.SwordPrefab;
                Sword sword = null;
                if (_swordEquipped != null)
                {
                    sword = _swordEquipped;
                }
                else if (swordPrefab != null)
                {
                    sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
                    EquipSword(sword);
                }

                onEquipSwordSucceed?.Invoke(sword);
            }

            _smoother.Tick(deltaTime);

            if (_swordEquipped != null)
            {
                MatchSwordToHandPosition();
            }

            _animationHandler.Tick(deltaTime);
            _cardUi.Tick(deltaTime);
        }

        private void FixedUpdate()
        {
            if (_swordEquipped != null)
            {
                MatchSwordToHandPosition();
            }
        }

        private void MatchSwordToHandPosition()
        {
            var swordTransform = _swordEquipped.transform;
            var swordCenterTransform = _swordEquipped.Center;
            var target = IkUtility.MatchStickWithHandlePoints(_swordHandleBottom, _swordHandleTop);
            var swordTransformData = IkUtility.MoveParentToMatchChild(swordTransform, swordCenterTransform, target);
            swordTransformData.ApplyTo(swordTransform);
        }

        private void UnEquipActiveSword()
        {
            Destroy(_swordEquipped.gameObject);
        }

        private void EquipSword(Sword swordInstance)
        {
            if (_swordEquipped != null)
            {
                Debug.LogError(
                    $"[Weapon Equip System] Player could not equip another sword" +
                    $" without first un-equipping the old one ({_swordEquipped.gameObject.name})");
                return;
            }

            _swordEquipped = swordInstance;
        }

        private static bool AnyActionDisablesFreeMovement(List<ActionEffects> actionEffects)
        {
            return actionEffects.Any(effect => !effect.FreeMovementEnabled);
        }

        private void UseSouthCard() => UseCard(0);

        private void UseEastCard() => UseCard(1);

        private void UseNorthCard() => UseCard(2);

        private void UseWestCard() => UseCard(3);

        private void UseCard(int index)
        {
            Debug.Assert(0 <= index && index < _cards.Count);

            if (_actionExecutor.HasPendingOrActiveActions)
            {
                return;
            }

            var useResult = _cards[index].Use(this);
            var action = useResult.Action;
            if (action != null)
            {
                _actionExecutor.AddAction(useResult.Action);
            }
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

        public void Visit(GreatSwordSlashAction greatSwordSlashAction)
        {
            greatSwordSlashAction.Init(_playerPawn, _animationHandler, _weaponTriggers);
        }

        public void Visit(ChargeAction chargeAction)
        {
            chargeAction.Init(_playerPawn);
        }
    }
}