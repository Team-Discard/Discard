using System.Collections.Generic;
using UnityEngine;

namespace SpawnerSystem
{
    public class StandardEnemySpawner : MonoBehaviour, IEnemySpawner
    {
        [SerializeField] private GameObject _prefab;
        
        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList)
        {
            outputList.Add(new EnemySpawnDesc
            {
                enemyPrefab = _prefab,
                position = transform.position
            });
        }

        public bool Completed => true;
    }
}