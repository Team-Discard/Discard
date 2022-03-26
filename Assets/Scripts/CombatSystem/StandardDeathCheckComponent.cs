using EntitySystem;
using UnityEngine;

namespace CombatSystem
{
    public class StandardDeathCheckComponent : 
        StandardComponent,
        IDeathCheckComponent
    {
        private readonly IHealthBarComponent _healthBar;
        private readonly GameObject _objectToDestroy;

        public StandardDeathCheckComponent(IHealthBarComponent healthBar, GameObject objectToDestroy)
        {
            _healthBar = healthBar;
            _objectToDestroy = objectToDestroy;
        }

        public void Tick(float deltaTime)
        {
            if (_healthBar.CurrentHealth <= 0.0f)
            {
                Object.Destroy(_objectToDestroy);
            }
        }
    }
}