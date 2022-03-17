using System.Collections.Generic;
using EntitySystem;
using UnityEngine;

namespace SpawnerSystem
{
    public class StandardEnemySpawner :
        GameObjectComponent<StandardEnemySpawner>,
        IEnemySpawner
    {
        [SerializeField] private GameObject _prefab;

        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList)
        {
            outputList.Add(new EnemySpawnDesc
            {
                enemyPrefab = _prefab,
                position = transform.position
            });
            Destroy();
        }
    }
}