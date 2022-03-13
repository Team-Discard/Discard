using System.Collections.Generic;

namespace SpawnerSystem
{
    public interface IEnemySpawner
    {
        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList);
        public bool Completed { get; }
    }
}