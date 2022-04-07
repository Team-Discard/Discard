using System.Collections.Generic;
using ActionSystem;
using Animancer;
using CameraSystem;
using CardSystem;
using CharacterSystem;
using CombatSystem;
using EntitySystem;
using MotionSystem;
using UI.HealthBar;
using UnityEngine;
using Unstable.Entities;
using Uxt.InterModuleCommunication;
using WeaponSystem;

namespace PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class StandardPlayer :
        GameObjectComponent, IRegisterSelf
    {
        [SerializeField] private StandardWeaponLocomotionAnimationSet _noWeaponAnimationSet;

        [SerializeField] private Transform _leftHandSocket;
        [SerializeField] private Transform _rightHandSocket;
        [SerializeField] private Transform _controlCamera;
        [SerializeField] private PlayerInputHandler _inputHandler;

        [SerializeField] private float _maxSpeed;

        [SerializeField] private StandardDamageTaker _damageTaker;
        [SerializeField] private StandardHealthBar _healthBar;
        [SerializeField] private CharacterCameraSetup _cameraSetup;
        [SerializeField] private CardManager _cardManager;
        
        private RootMotionSource _rootMotionSource;
        private IPawnComponent _pawn;
        private IActionExecutorComponent _actionExecutor;
        private PawnAnimationHandler _animationHandler;
        private IPawnControllerComponent _controller;
        private IWeaponEquipComponent _weaponEquipHandler;
        private ICardUserComponent _cardUser;
        private CardButtonHandler _cardButtonHandler;
        private IDeathCheckComponent _deathCheckComponent;
        private IHealthBarWatcherComponent _healthBarWatcher;

        // todo: to:billy implement a proper system for giving player cards for debug and testing purposes
        [SerializeField] private List<Card> _debugPlayerCards;

        public IHealthBarComponent HealthBar => _healthBar;

        public override void Init()
        {
            base.Init();

            var animancer = GetComponentInChildren<AnimancerComponent>();
            Debug.Assert(animancer != null, "Player must have an animancer component in its children");

            _rootMotionSource = GetComponentInChildren<RootMotionSource>();
            Debug.Assert(_rootMotionSource != null, "Player must have an root motion frame in its children");

            _pawn = new CharacterControllerPawn(GetComponent<CharacterController>());
            _actionExecutor = new ActionExecutor();
            _animationHandler = new PawnAnimationHandler(_pawn, animancer, _noWeaponAnimationSet);
            _weaponEquipHandler = new StandardWeaponEquipHandler(
                _leftHandSocket,
                _rightHandSocket,
                _animationHandler);
            _controller =
                new StandardPlayerController(_pawn, _controlCamera, _actionExecutor, _inputHandler, _maxSpeed);

            foreach (var card in _debugPlayerCards)
            {
                _cardManager.AcquireCard(card, 0);
            }
            
            _cardUser = new StandardCardUser(_actionExecutor, _cardManager);

            _healthBar = GetComponent<StandardHealthBar>();
            _damageTaker.BindHealthBar(_healthBar);

            _deathCheckComponent = new StandardDeathCheckComponent(_healthBar, gameObject);

            var cardUseDependencies = new DependencyBag
            {
                _weaponEquipHandler,
                _animationHandler,
                _rootMotionSource,
                transform,
                GetComponent<SocketGroup>()
            };

            _cardButtonHandler = new CardButtonHandler(_inputHandler, _cardUser, cardUseDependencies);

            _inputHandler.onToggleLockOn += () =>
            {
                if (_cameraSetup.CurrentMode != CharacterCameraMode.TargetLockOn)
                {
                    _cameraSetup.BeginLockOn(GameObject.FindGameObjectWithTag("Enemy").transform.Find("Lock On Point"));
                }
                else
                {
                    _cameraSetup.EndLockOn();
                }
            };
        }

        public void RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(_damageTaker);
            registry.AddComponent(_pawn);
            registry.AddComponent(_actionExecutor);
            registry.AddComponent(_animationHandler);
            registry.AddComponent(_weaponEquipHandler);
            registry.AddComponent(_controller);
            registry.AddComponent(_cardUser);
            registry.AddComponent(_cardButtonHandler);
            registry.AddComponent(_healthBar);
            registry.AddComponent(_deathCheckComponent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _pawn.Destroy();
            _actionExecutor.Destroy();
            _animationHandler.Destroy();
            _weaponEquipHandler.Destroy();
            _controller.Destroy();
            _cardUser.Destroy();
            _cardButtonHandler.Destroy();
            _healthBar.Destroy();
            _deathCheckComponent.Destroy();
        }

        public void BindControlCamera(Transform controlCamera)
        {
            _controlCamera = controlCamera;
        }
    }
}