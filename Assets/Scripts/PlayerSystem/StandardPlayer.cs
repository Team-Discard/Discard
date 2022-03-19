using System.Collections.Generic;
using ActionSystem;
using Animancer;
using CardSystem;
using CharacterSystem;
using EntitySystem;
using UnityEngine;
using Unstable.Entities;
using Uxt;
using WeaponSystem;

namespace PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class StandardPlayer :
        GameObjectComponent, IComponentSource
    {
        [SerializeField] private StandardWeaponLocomotionAnimationSet _noWeaponAnimationSet;

        [SerializeField] private Transform _leftHandSocket;
        [SerializeField] private Transform _rightHandSocket;

        [SerializeField] private Transform _controlCamera;

        [SerializeField] private PlayerInputHandler _inputHandler;

        [SerializeField] private float _maxSpeed;

        // to:billy todo: hand should not be stored on standard player
        [SerializeField] private List<Card> _hand;

        private RootMotionFrame _rootMotionFrame;

        private IPawn _pawn;
        private IActionExecutor _actionExecutor;
        private PawnAnimationHandler _animationHandler;
        private IPawnController _controller;
        private IWeaponEquipHandler _weaponEquipHandler;
        private TemporaryCardTextUI _cardUi;
        private ICardUser _cardUser;
        private CardButtonHandler _cardButtonHandler;

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

            var cardUseDependencies = new DependencyBag
            {
                _weaponEquipHandler,
                _animationHandler,
                _rootMotionFrame
            };

            _cardButtonHandler = new CardButtonHandler(_inputHandler, _cardUser, cardUseDependencies);
        }

        public IEnumerable<IComponent> AllComponents
        {
            get
            {
                yield return _pawn;
                yield return _actionExecutor;
                yield return _animationHandler;
                yield return _weaponEquipHandler;
                yield return _controller;
                yield return _cardUi;
                yield return _cardUser;
                yield return _cardButtonHandler;
            }
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
        }
    }
}