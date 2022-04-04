using System.Collections.Generic;
using EntitySystem;
using SpawnerSystem;
using UnityEngine;
using Unstable;

namespace FlowControl
{
    public class LevelFlow :
        ITicker
    {
        private enum LevelState
        {
            Undiscovered,
            InCombat,
            Completed
        }

        private readonly IComponentRegistry _gameCompRegistry;
        private readonly ComponentList<IEnemySpawnerComponent> _enemySpawners = new();
        private readonly ComponentList<IEnemyComponent> _enemies = new();
        private readonly List<EnemySpawnDesc> _enemySpawnBuffer = new();

        private LevelState _state;

        private GameObject _rewardChest;

        public LevelFlow(IComponentRegistry gameCompRegistry, GameObject levelRoot)
        {
            _gameCompRegistry = gameCompRegistry;
            _rewardChest = levelRoot.transform.Find("Reward Chest")?.gameObject;

            Entity.SetUp(levelRoot.transform, c =>
            {
                if (c.IsComponentOfType(out IEnemySpawnerComponent spawner))
                {
                    _enemySpawners.Add(spawner);
                }
                else
                {
                    _gameCompRegistry.AddComponent(c);
                }
            });

            _state = LevelState.InCombat;
        }

        public void Tick(float deltaTime)
        {
            _enemySpawnBuffer.Clear();
            _enemySpawners.Tick(
                deltaTime,
                (spawner, dt) => spawner.TickSpawner(dt, _enemySpawnBuffer));
            
            foreach (var spawnDesc in _enemySpawnBuffer)
            {
                var enemyObj = Object.Instantiate(
                    spawnDesc.enemyPrefab,
                    spawnDesc.position,
                    Quaternion.identity);
                
                Entity.SetUp(enemyObj.transform, c =>
                {
                    if (c.IsComponentOfType(out IEnemyComponent enemy))
                    {
                        _enemies.Add(enemy);
                    }
                    _gameCompRegistry.AddComponent(c);
                });
            }

            _enemies.RemoveDestroyed();
            
            if (_state == LevelState.InCombat &&
                _enemySpawners.Count == 0 &&
                _enemies.Count == 0)
            {
                _state = LevelState.Completed;
                SpawnReward();
            }
        }

        private void SpawnReward()
        {
            if (_rewardChest != null)
            {
                _rewardChest.SetActive(true);
            }
        }
    }
}