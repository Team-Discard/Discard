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

        private readonly ComponentList<IEnemySpawner> _enemySpawners = new();
        private readonly ComponentList<IEnemy> _enemies = new();
        private readonly List<EnemySpawnDesc> _enemySpawnBuffer = new();

        private LevelState _state;
        
        private GameObject _rewardChest;

        public LevelFlow(GameObject levelRoot)
        {
            _rewardChest = levelRoot.transform.Find("Reward Chest")?.gameObject;
            foreach (var spawner in levelRoot.transform.GetComponentsInChildren<IEnemySpawner>())
            {
                _enemySpawners.Add(spawner);
            }

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
                var enemy = EntityUtils.Instantiate<IEnemy>(
                    spawnDesc.enemyPrefab, 
                    spawnDesc.position,
                    Quaternion.identity);
                ComponentRegistry.AddEntity(enemy);
                _enemies.Add(enemy);
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