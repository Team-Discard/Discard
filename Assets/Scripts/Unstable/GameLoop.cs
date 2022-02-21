using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unstable.Entities;

namespace Unstable
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private PlayerMasterController _playerController;

        private List<IDamageTaker> _damageTakers;
        private List<IEnemy> _enemies;
        private List<IPhysicsTicker> _physicsTickers;


        private void Start()
        {
            _damageTakers = FindObjectsOfType<Component>()
                .OfType<IDamageTaker>()
                .ToList();

            _enemies = FindObjectsOfType<Component>()
                .OfType<IEnemy>()
                .ToList();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            _playerController.Tick(deltaTime);

            var actionEffects = _playerController.CurrentActionEffects;

            foreach (var enemyController in _damageTakers)
            {
                foreach (var actionEffect in actionEffects)
                {
                    if (actionEffect.DamageVolumes == null) continue;
                    foreach (var damageVolume in actionEffect.DamageVolumes)
                    {
                        enemyController.InteractWithDamageVolume(damageVolume);
                    }
                }
            }

            foreach (var enemyController in _damageTakers)
            {
                enemyController.ReckonAllDamage(deltaTime);
            }
            
            foreach (var enemy in _enemies)
            {
                enemy.Tick(deltaTime);
            }
        }
    }
}