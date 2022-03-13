using System.Collections.Generic;
using SpawnerSystem;
using UnityEngine;
using Unstable;

namespace FlowControl
{
    public class LevelFlow : ITicker
    {
        private enum LevelEvent
        {
            EnterLevel,
            AllEnemiesDefeated
        }

        private enum LevelState
        {
            Undiscovered,
            InCombat,
            Completed
        }

        private readonly List<IEnemySpawner> _enemySpawners;
        private readonly List<EnemySpawnDesc> _enemySpawnDescBuffer = new();
        private readonly List<IEnemy> _enemies;

        private readonly Queue<LevelEvent> _events = new();
        private LevelState _state;

        public void Tick(float deltaTime)
        {
            TickEnemySpawners(deltaTime, _enemySpawners, _enemySpawnDescBuffer);
            SpawnEnemies(_enemySpawnDescBuffer);
            RemovedCompletedSpawners();
            if (_enemySpawners.Count == 0)
            {
                EmitEvent(LevelEvent.AllEnemiesDefeated);
            }
        }

        private void SpawnEnemies(List<EnemySpawnDesc> spawnDescriptions)
        {
            foreach (var spawnDesc in spawnDescriptions)
            {
                var go = Object.Instantiate(spawnDesc.enemyPrefab, spawnDesc.position, Quaternion.identity);
                var enemy = go.GetComponent<IEnemy>();
                AddEnemy(enemy);
            }
        }

        private void AddEnemy(IEnemy enemy)
        {
            _enemies.Add(enemy);
        }

        private void EmitEvent(LevelEvent e)
        {
            _events.Enqueue(e);
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

        private void RemovedCompletedSpawners()
        {
            _enemySpawners.RemoveAll(spawner => spawner.Completed);
        }
    }
}