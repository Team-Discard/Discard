using System.Collections.Generic;
using ActionSystem;
using Animancer;
using CardSystem;
using CharacterSystem;
using CombatSystem;
using EntitySystem;
using UnityEngine;
using Unstable.Entities;
using Uxt;
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

        // to:billy todo: hand should not be stored on standard player
        [SerializeField] private List<Card> _hand;

        [SerializeField] private StandardDamageTaker _damageTaker;
        
        private RootMotionFrame _rootMotionFrame;

        private IPawnComponent _pawn;
        private IActionExecutorComponent _actionExecutor;
        private PawnAnimationHandler _animationHandler;
        private IPawnControllerComponent _controller;
        private IWeaponEquipComponent _weaponEquipHandler;
        private TemporaryCardTextUI _cardUi;
        private ICardUserComponent _cardUser;
        private CardButtonHandler _cardButtonHandler;
        private StandardHealthBar _healthBar;
        private IDeathCheckComponent _deathCheckComponent;

        public override void Init()
        {
            base.Init();

            var animancer = GetComponentInChildren<AnimancerComponent>();
            Debug.Assert(animancer != null, "Player must have an animancer component in its children");

            _rootMotionFrame = GetComponentInChildren<RootMotionFrame>();
            Debug.Assert(_rootMotionFrame != null, "Player must have an root motion frame in its children");

            _pawn = new CharacterControllerPawn(GetComponent<CharacterController>());
            _actionExecutor = new ActionExecutor();
            _animationHandler = new PawnAnimationHandler(_pawn, animancer, _noWeaponAnimationSet);
            _weaponEquipHandler = new StandardWeaponEquipHandler(
                _leftHandSocket,
                _rightHandSocket,
                _animationHandler);
            _controller =
                new StandardPlayerController(_pawn, _controlCamera, _actionExecutor, _inputHandler, _maxSpeed);

            _cardUi = new TemporaryCardTextUI(_hand);
            _cardUser = new StandardCardUser(_actionExecutor, _hand);
            
            _healthBar = new StandardHealthBar(10.0f, _damageTaker);
            _deathCheckComponent = new StandardDeathCheckComponent(_healthBar, gameObject);

            var cardUseDependencies = new DependencyBag
            {
                _weaponEquipHandler,
                _animationHandler,
                _rootMotionFrame,
                transform
            };

            _cardButtonHandler = new CardButtonHandler(_inputHandler, _cardUser, cardUseDependencies);
        }

        public void RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(_damageTaker);            
            registry.AddComponent(_pawn);
            registry.AddComponent(_actionExecutor);
            registry.AddComponent(_animationHandler);
            registry.AddComponent(_weaponEquipHandler);
            registry.AddComponent(_controller);
            registry.AddComponent(_cardUi);
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
            _cardUi.Destroy();
            _cardUser.Destroy();
            _cardButtonHandler.Destroy();
            _healthBar.Destroy();
            _deathCheckComponent.Destroy();
        }
    }
}