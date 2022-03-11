using System.Collections.Generic;
using System.Linq;
using CombatSystem;
using UnityEngine;

namespace Unstable.Entities
{
    public class SlimeEnemyController :
        MonoBehaviour,
        IEnemy,
        IDamageTaker
    {
        [SerializeField] private List<HurtBox> _damageableParts;
        [SerializeField] private float _invincibleDuration;
        [SerializeField] private DamageLayer _damageLayer;

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

        public void HandleDamage(IDamageBox damageBox)
        {
            if (!IsHitBy(damageBox))
            {
                return;
            }

            var damage = damageBox.GetDamage();

            if (damage.Layer != _damageLayer)
            {
                return;
            }

            _totalDamage += damageBox.GetDamage().BaseAmount;
            _hasTakenDamage = true;
        }

        private bool IsHitBy(IDamageBox damageBox)
        {
            return _damageableParts.Any(damageBox.CheckOverlap);
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