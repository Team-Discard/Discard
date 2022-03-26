using System.Collections.Generic;
using ActionSystem;
using CardSystem;
using CharacterSystem;
using CombatSystem;
using EntitySystem;
using PlayerSystem;
using SpawnerSystem;
using UnityEngine;
using Unstable;
using Unstable.Entities;
using WeaponSystem;

namespace FlowControl
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private StandardPlayer _player;

        private GameObject _currentLevelRoot;
        private LevelFlow _currentLevelFlow;

        private List<EnemySpawnDesc> _enemySpawnBuffer;
        private ComponentRegistry _componentRegistry;

        private void Awake()
        {
            _enemySpawnBuffer = new List<EnemySpawnDesc>();
            _componentRegistry =
                new ComponentRegistry()
                    .AllowType<IEnemyComponent>()
                    .AllowType<IPawnComponent>()
                    .AllowType<PawnAnimationHandler>()
                    .AllowType<IDamageTakerComponent>()
                    .AllowType<IHealthBar>()
                    .AllowType<IPawnControllerComponent>()
                    .AllowType<IWeaponEquipComponent>()
                    .AllowType<IActionExecutorComponent>()
                    .AllowType<IDeathCheckComponent>()
                    .AllowType<IPrototypeComponent>();
        }

        private void Start()
        {
            Entity.SetUp(
                _player.transform,
                c =>
                {
                    if (c.IsComponentOfType<ICardUserComponent>())
                    {
                        return;
                    }

                    if (c.IsComponentOfType<IPawnComponent>())
                    {
                        print("Pawn!");
                    }
                    _componentRegistry.Add(c);
                });
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
            }

            _componentRegistry
                .Get<IActionExecutorComponent>()
                .Tick(deltaTime, (ae, dt) => ae.Execute(dt));

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
                .Get<IHealthBar>()
                .Tick(deltaTime, (hb, dt) => hb.Tick(dt));
            
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

        private ComponentList<IPawnComponent> _pawns;
        private ComponentList<PawnAnimationHandler> _animationHandlers;
    }
}