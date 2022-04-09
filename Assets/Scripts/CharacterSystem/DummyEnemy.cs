using EntitySystem;

namespace CharacterSystem
{
    public class DummyEnemy : GameObjectComponent, IEnemyComponent
    {
        public void Tick(float deltaTime)
        {
        }
    }
}