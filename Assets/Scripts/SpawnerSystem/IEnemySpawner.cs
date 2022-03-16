using System.Collections.Generic;
using EntitySystem;

namespace SpawnerSystem
{
    public interface IEnemySpawner : IEntity, IComponent
    {
        public void TickSpawner(float deltaTime, List<EnemySpawnDesc> outputList);
    }
}