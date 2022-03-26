using System.Collections.Generic;
using EntitySystem;

namespace SpawnerSystem
{
    public interface IEnemySpawnerComponent : IComponent<IEnemySpawnerComponent>
    {
        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList);
    }
}