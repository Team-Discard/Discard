using System.Collections.Generic;
using System.Linq;
using CombatSystem;
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
        private List<EnemyController> _enemyControllers;

        #region List Buffers

        private List<DamageIdPair> _damageBuffer;

        #endregion

        private void Awake()
        {
            _damageBuffer = new List<DamageIdPair>();
        }

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

            DamageManager.GetAllDamages(_damageBuffer);
            foreach (var damageTaker in _damageTakers)
            {
                foreach (var pair in _damageBuffer)
                {
                    damageTaker.HandleDamage(pair.Id, pair.Damage);
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
            
            DamageManager.TickInvincibilityFrames(deltaTime);
        }
    }
}