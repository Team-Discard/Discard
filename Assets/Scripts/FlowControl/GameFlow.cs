﻿using System;
using System.Collections.Generic;
using CombatSystem;
using EntitySystem;
using SpawnerSystem;
using UnityEngine;
using Unstable;
using Unstable.Entities;

namespace FlowControl
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private PlayerMasterController _playerController;

        private GameObject _currentLevelRoot;
        private LevelFlow _currentLevelFlow;

        private List<EnemySpawnDesc> _enemySpawnBuffer;
        private ComponentRegistry _componentRegistry;

        private void Awake()
        {
            _enemySpawnBuffer = new List<EnemySpawnDesc>();
            _componentRegistry =
                new ComponentRegistry()
                    .AllowType<IEnemy>()
                    .AllowType<IPawn>()
                    .AllowType<PawnAnimationHandler>()
                    .AllowType<IDamageTaker>()
                    .AllowType<IHealthBar>();
        }

        private void Start()
        {
            Entity.SetUp(_playerController.gameObject, c => _componentRegistry.Add(c));
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
            
            _playerController.Tick(deltaTime);

            _componentRegistry
                .Get<IEnemy>()
                .Tick(deltaTime, (enemy, dt) => enemy.Tick(dt));

            _componentRegistry
                .Get<IPawn>()
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
            DamageManager.ResolveDamages(_componentRegistry.Get<IDamageTaker>());

            _componentRegistry
                .Get<PawnAnimationHandler>()
                .Tick(deltaTime, (handler, dt) => handler.Tick(dt));
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