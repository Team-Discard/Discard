using System.Collections.Generic;
using EntitySystem;
using UnityEditor;
using UnityEngine;

namespace SpawnerSystem
{
    public class StandardEnemySpawner :
        MonoBehaviour,
        IEnemySpawner
    {
        [SerializeField] private GameObject _prefab;

        private void Awake()
        {
            Destroyed = false;
        }

        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList)
        {
            outputList.Add(new EnemySpawnDesc
            {
                enemyPrefab = _prefab,
                position = transform.position
            });
            Destroy();
        }

        public IEntity Entity => this;

        public void AddTo(IComponentRegistry registry)
        {
            registry.AddEnemySpawner(this);
        }

        public bool Destroyed { get; private set; }

        public void Destroy()
        {
            Destroyed = true;
            Destroy(gameObject);
        }
    }
}