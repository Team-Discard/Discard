using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unstable.Entities
{
    public class SlimeEnemyController : 
        MonoBehaviour, 
        IEnemy,
        IDamageTaker
    {
        [SerializeField] private List<DamageablePart> _damageableParts;
        [SerializeField] private float _invincibleDuration;
        
        private float _totalDamage;
        private float _invincibilityTimer;
        private bool _hasTakenDamage;
        
        private void Awake()
        {
            _totalDamage = 0.0f;
            _invincibilityTimer = 0.0f;
            _hasTakenDamage = false;
        }

        public void Tick(float deltaTime)
        {
            _invincibilityTimer -= Time.deltaTime;
        }

        public void InteractWithDamageVolume(IDamageVolume damageVolume)
        {
            if (IsHitBy(damageVolume))
            {
                // todo: evil null here
                _totalDamage += damageVolume.GetDamageAmount(null);
                _hasTakenDamage = true;
            }
        }

        private bool IsHitBy(IDamageVolume damageVolume)
        {
            return _damageableParts.Any(damageVolume.CheckOverlap);
        }
        
        public void ReckonAllDamage(float deltaTime)
        {
            if (!_hasTakenDamage)
            {
                return;
            }
            
            if (_invincibilityTimer > 0.0f)
            {
                _totalDamage = 0.0f;
                _hasTakenDamage = false;
                return;
            }
            _invincibilityTimer = _invincibleDuration;
            
            Debug.Log($"{gameObject.name} has taken {_totalDamage} damage");

            _hasTakenDamage = false;
            _totalDamage = 0.0f;
        }
    }
}