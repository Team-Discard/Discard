using System.Collections.Generic;
using EntitySystem;

namespace SpawnerSystem
{
    public interface IEnemySpawner : IComponent<IEnemySpawner>
    {
        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList);
    }
}