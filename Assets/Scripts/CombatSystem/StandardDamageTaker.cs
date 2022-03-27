using System.Collections.Generic;
using EntitySystem;
using UnityEngine;

namespace CombatSystem
{
    public class StandardDamageTaker :
        MonoBehaviourComponent,
        IDamageTakerComponent
    {
        [SerializeField] private HurtBox _hurtBox;
        [SerializeField] private float _invincibilityFrame;
        [SerializeField] private DamageLayer _damageLayer;
        private IHealthBarComponent _healthBar;
        private Queue<Damage> _damageQueue;

        private void Awake()
        {
            _damageQueue = new Queue<Damage>();
        }

        public void BindHealthBar(IHealthBarComponent healthBar)
        {
            Debug.Assert(_healthBar == null, "_healthBar == null");
            _healthBar = healthBar;
        }
        
        public void HandleDamage(int id, in Damage damage)
        {
            if (!damage.DamageBox.CheckOverlap(_hurtBox))
            {
                return;
            }

            // Cannot take friendly damage
            if (damage.Layer == _damageLayer)
            {
                return;
            }

            // Cannot take damage while in invincibility frame
            if (!DamageManager.SetInvincibilityFrame(this, id, _invincibilityFrame))
            {
                return;
            }

            _damageQueue.Enqueue(damage);
        }

        public void ReckonAllDamage()
        {
            Debug.Assert(_healthBar != null, "The damage taker must reference a health bar", gameObject);
            while (_damageQueue.TryDequeue(out var dmg))
            {
                _healthBar.CurrentHealth -= dmg.BaseAmount;
            }
        }
    }
}