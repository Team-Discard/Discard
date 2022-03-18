using System.Collections.Generic;
using ActionSystem;
using Animancer;
using CharacterSystem;
using EntitySystem;
using UnityEngine;
using Unstable.Entities;
using WeaponSystem;

namespace PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class StandardPlayer : GameObjectComponent, IComponentSource
    {
        [SerializeField] private StandardWeaponLocomotionAnimationSet _noWeaponAnimationSet;

        [SerializeField] private Transform _leftHandSocket;
        [SerializeField] private Transform _rightHandSocket;

        [SerializeField] private Transform _controlCamera;

        [SerializeField] private PlayerInputHandler _inputHandler;

        [SerializeField] private float _maxSpeed;
        
        private IPawn _pawn;
        private IActionExecutor _actionExecutor;
        private PawnAnimationHandler _animationHandler;
        private IPawnController _controller;
        private IWeaponEquipHandler _weaponEquipHandler;

        public override void Init()
        {
            base.Init();

            var animancer = GetComponentInChildren<AnimancerComponent>();
            Debug.Assert(animancer != null, "Player must have an animancer component in its children");

            _pawn = new CharacterControllerPawn(GetComponent<CharacterController>());
            _actionExecutor = new ActionExecutor();
            _animationHandler = new PawnAnimationHandler(_pawn, animancer, _noWeaponAnimationSet);
            _weaponEquipHandler = new StandardWeaponEquipHandler(_leftHandSocket, _rightHandSocket);
            _controller = new StandardPlayerController(_pawn, _controlCamera, _actionExecutor, _inputHandler, _maxSpeed);
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
        }
    }
}