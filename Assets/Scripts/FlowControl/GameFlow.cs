using System.Collections.Generic;
using CombatSystem;
using EntitySystem;
using SpawnerSystem;
using UnityEngine;
using Unstable.Entities;

namespace FlowControl
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private PlayerMasterController _playerController;

        private GameObject _currentLevelRoot;
        private LevelFlow _currentLevelFlow;

        private List<EnemySpawnDesc> _enemySpawnBuffer;

        private void Awake()
        {
            _enemySpawnBuffer = new List<EnemySpawnDesc>();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (PlayerEnteredNewLevel(out var newLevelRoot))
            {
                // We would not handle the situation where the player switch levels, YET
                Debug.Assert(_currentLevelFlow == null);

                _currentLevelRoot = newLevelRoot;
                _currentLevelFlow = new LevelFlow(newLevelRoot);
            }

            _playerController.Tick(deltaTime);

            ComponentRegistry.Enemies.Tick(deltaTime, (enemy, dt) => enemy.Tick(dt));
            ComponentRegistry.Pawns.Tick(deltaTime, (pawn, dt) =>
            {
                pawn.TickRotation(dt);
                pawn.TickTranslation(dt);
            });

            if (_currentLevelFlow != null)
            {
                _currentLevelFlow.Tick(deltaTime);
            }

            DamageManager.TickInvincibilityFrames(deltaTime);
            DamageManager.ResolveDamages();
            ComponentRegistry.AnimationHandlers.Tick(deltaTime, (handler, dt) => handler.Tick(dt));
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

        private ComponentList<IPawn> _pawns;
        private ComponentList<PawnAnimationHandler> _animationHandlers;
    }
}