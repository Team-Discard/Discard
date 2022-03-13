using System.Collections.Generic;
using System.Linq;
using CombatSystem;
using EntitySystem;
using SpawnerSystem;
using UnityEngine;
using Unstable;

namespace FlowControl
{
    public class LevelFlow :
        ITicker, IComponentRegistry
    {
        private enum LevelState
        {
            Undiscovered,
            InCombat,
            Completed
        }

        private readonly List<IEnemySpawner> _enemySpawners;
        private readonly List<EnemySpawnDesc> _enemySpawnDescBuffer = new();
        private readonly List<IEnemy> _enemies = new();

        private LevelState _state;

        private GameObject _rewardChest;

        public LevelFlow(GameObject levelRoot)
        {
            _enemySpawners = levelRoot.GetComponentsInChildren<IEnemySpawner>().ToList();
            _rewardChest = levelRoot.transform.Find("Reward Chest")?.gameObject;
            _state = LevelState.InCombat;
        }

        public void Tick(float deltaTime)
        {
            TickEnemySpawners(deltaTime, _enemySpawners, _enemySpawnDescBuffer);
            SpawnEnemies(_enemySpawnDescBuffer);
            _enemySpawners.RemoveAll(spawner => spawner.Completed);

            TickEnemies(deltaTime);
            _enemies.RemoveAll(enemy => enemy.Defeated);

            if (_state == LevelState.InCombat &&
                _enemySpawners.Count == 0 && _enemies.Count == 0)
            {
                _state = LevelState.Completed;
                SpawnReward();
            }
        }

        private void TickEnemies(float deltaTime)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Tick(deltaTime);
            }
        }

        private void SpawnReward()
        {
            if (_rewardChest != null)
            {
                _rewardChest.SetActive(true);
            }
        }

        private void SpawnEnemies(List<EnemySpawnDesc> spawnDescriptions)
        {
            foreach (var spawnDesc in spawnDescriptions)
            {
                var go = Object.Instantiate(spawnDesc.enemyPrefab, spawnDesc.position, Quaternion.identity);
                var enemyEntity = go.GetComponent<IEntity>();
                enemyEntity.AddTo(this);
            }
        }

        private void TickEnemySpawners(
            float deltaTime,
            List<IEnemySpawner> enemySpawners,
            List<EnemySpawnDesc> enemySpawnDescBuffer)
        {
            enemySpawnDescBuffer.Clear();
            foreach (var enemySpawner in enemySpawners)
            {
                enemySpawner.TickSpawner(deltaTime, _enemySpawnDescBuffer);
            }
        }

        bool IComponentRegistry.AddEnemy(IEnemy enemy)
        {
            _enemies.Add(enemy);
            return true;
        }
    }
}