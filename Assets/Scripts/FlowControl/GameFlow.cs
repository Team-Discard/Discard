using System.Collections.Generic;
using System.Linq;
using ActionSystem;
using CardSystem;
using CharacterSystem;
using CombatSystem;
using Debugging;
using EntitySystem;
using InteractionSystem;
using PlayerSystem;
using ProjectileSystem;
using SpawnerSystem;
using UI;
using UI.HealthBar;
using UnityEngine;
using Unstable;
using Unstable.Entities;
using Uxt.Debugging;
using Uxt.PropertyDrawers;
using WeaponSystem;

namespace FlowControl
{
    public class GameFlow : MonoBehaviour, IComponentRegistry
    {
        [SerializeField] private StandardPlayer _player;
        [SerializeField] private PlayerStatsDisplay _playerStatsDisplay;
        [SerializeField] private NpcHealthBarRendererManager _npcHealthBarRendererMgr;
        
        private GameObject _currentLevelRoot;
        private LevelFlow _currentLevelFlow;

        private List<EnemySpawnDesc> _enemySpawnBuffer;

        private List<IInteractable>
            _interactables; // list of interactables, optimally should be handled by each level but it is here for now

        [SerializeField, EditInPrefabOnly] private float interactableScanRange;
        private ComponentRegistry _componentRegistry;

        private void Awake()
        {
            DebugConsole.RegisterHandler(
                "t",
                DebugConsole.CreateHandlerFromObject(new TimeScaler()));

            _enemySpawnBuffer = new List<EnemySpawnDesc>();
            _componentRegistry =
                new ComponentRegistry()
                    .AllowType<IEnemyComponent>()
                    .AllowType<IPawnComponent>()
                    .AllowType<PawnAnimationHandler>()
                    .AllowType<IDamageTakerComponent>()
                    .AllowType<IHealthBarComponent>()
                    .AllowType<IPawnControllerComponent>()
                    .AllowType<IWeaponEquipComponent>()
                    .AllowType<IActionExecutorComponent>()
                    .AllowType<IDeathCheckComponent>()
                    .AllowType<IPrototypeComponent>()
                    .AllowType<IHealthBarWatcherComponent>()
                    .AllowType<IHealthBarRendererComponent>()
                    .AllowType<ICardUserComponent>()
                    .AllowType<IHealthBarTransformComponent>()
                    .AllowType<FollowConstraintComponent>()
                    .AllowType<ProjectileComponent>();

            // todo: to:billy get rid of this singleton
            ComponentRegistry.Instance = _componentRegistry;

            if (_npcHealthBarRendererMgr != null)
            {
                _npcHealthBarRendererMgr.BindComponentRegistry(this);
            }
        }

        private void Start()
        {
            Entity.SetUp(
                _player.transform,
                AddComponent);

            Entity.SetUp(
                _playerStatsDisplay.transform,
                AddComponent
            );

            _playerStatsDisplay.BindHealthBar(_player.HealthBar);

            // fill in the interactables
            _interactables = GetAllInteractables();

            // log all interactable object names
            foreach (var i in _interactables)
            {
                Debug.Log(i.MyGameObject.name);
            }
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (PlayerEnteredNewLevel(out var newLevelRoot))
            {
                // We would not handle the situation where the player switch levels, YET
                Debug.Assert(_currentLevelFlow == null);

                _currentLevelRoot = newLevelRoot;
                _currentLevelFlow = new LevelFlow(_componentRegistry, newLevelRoot);
                
                // todo: to:billy evil, refactor this
                _currentLevelRoot.BroadcastMessage("OnLevelCompleted");
            }

            _componentRegistry
                .Get<IActionExecutorComponent>()
                .Tick(deltaTime, (ae, dt) => ae.ExecuteAllActions(dt));

            _componentRegistry
                .Get<IPawnControllerComponent>()
                .Tick(deltaTime, (pc, dt) => pc.Tick(dt));

            _componentRegistry
                .Get<IEnemyComponent>()
                .Tick(deltaTime, (enemy, dt) => enemy.Tick(dt));

            _componentRegistry
                .Get<IPawnComponent>()
                .Tick(deltaTime, (pawn, dt) =>
                {
                    pawn.TickRotation(dt);
                    pawn.TickTranslation(dt);
                });

            if (_currentLevelFlow != null)
            {
                _currentLevelFlow.Tick(deltaTime);
            }

            DamageManager.TickInvincibilityFrames(deltaTime);
            DamageManager.ResolveDamages(_componentRegistry.Get<IDamageTakerComponent>());

            _componentRegistry
                .Get<IWeaponEquipComponent>()
                .Tick(deltaTime, (eh, dt) => eh.Tick(dt));

            _componentRegistry
                .Get<PawnAnimationHandler>()
                .Tick(deltaTime, (handler, dt) => handler.Tick(dt));

            _componentRegistry
                .Get<IPrototypeComponent>()
                .Tick(deltaTime, (p, dt) => p.Tick(dt));

            _componentRegistry
                .Get<IDeathCheckComponent>()
                .Tick(deltaTime, (dc, dt) => dc.Tick(dt));

            _componentRegistry
                .Get<IHealthBarWatcherComponent>()
                .Tick(deltaTime, (hbw, dt) => hbw.Tick(dt));

            _componentRegistry
                .Get<IHealthBarRendererComponent>()
                .Tick(deltaTime, (hbr, dt) => hbr.Tick(dt));

            _componentRegistry
                .Get<FollowConstraintComponent>()
                .Tick(deltaTime, (pcc, dt) => pcc.Tick(dt));

            if (_npcHealthBarRendererMgr != null)
            {
                _npcHealthBarRendererMgr.Tick(_componentRegistry.Get<IHealthBarTransformComponent>());
            }

            DebugMessageManager.AddOnScreen(
                $"Enemy count = {_componentRegistry.Get<IHealthBarTransformComponent>().Count}",
                "enemy_count".GetHashCode(), Color.cyan);

            if (InteractionManager.Instance != null && !InteractionEventSystem.IsInteracting)
            {
                // Scan for interactable and set it in interactionManager
                _interactables = GetAllInteractables();
                InteractionManager.Instance.SetCurrentFocusedInteractable(
                    ScanForClosestInteractableWithInRange(interactableScanRange));
                InteractionManager.Instance.DisplayInteractionHintIfNeeded();
            }
        }


        private bool PlayerEnteredNewLevel(out GameObject levelRoot)
        {
            levelRoot = _currentLevelRoot;

            // Technically there could be more than one level in the queue
            // but for now we only really care about the latest one
            while (LevelEnterManager.TryDequeueEnterLevelEvent(out var enteredLevel))
            {
                levelRoot = enteredLevel;
            }

            return levelRoot != _currentLevelRoot;
        }

        // function to get all interactable objects in the scene (inactive ones included)
        private static List<IInteractable> GetAllInteractables()
        {
            var interactablesFound = FindObjectsOfType<MonoBehaviour>(true).OfType<IInteractable>();
            return interactablesFound.ToList();
        }

        // function to find the closest interactable object within certain range of the player
        // todo: to:george from:billy we also need a way for interactables and interactors to specify an offset from
        //        their origin
        private IInteractable ScanForClosestInteractableWithInRange(float range)
        {
            IInteractable retVal = null;
            var closestDistanceSq = Mathf.Infinity;

            // find the closest interactable within range
            foreach (var interactable in _interactables)
            {
                // to:george This skips disabled interactables. Needed for conditionally enabling warp points
                if (interactable is MonoBehaviour { isActiveAndEnabled: false }) continue;

                var directionToTarget =
                    interactable.MyGameObject.transform.position - _player.gameObject.transform.position;

                var dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSq)
                {
                    closestDistanceSq = dSqrToTarget;

                    // if close enough to both player and screen center, set the interactable to be return value
                    if (dSqrToTarget <= range * range)
                    {
                        retVal = interactable;
                    }
                }
            }

            return retVal;
        }

        private ComponentList<IPawnComponent> _pawns;
        private ComponentList<PawnAnimationHandler> _animationHandlers;

        public void AddComponent(IComponent component)
        {
            _componentRegistry.AddComponent(component);
        }
    }
}