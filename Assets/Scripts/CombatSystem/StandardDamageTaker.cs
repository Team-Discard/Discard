using System.Collections.Generic;
using EntitySystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace CombatSystem
{
    public class StandardDamageTaker :
        MonoBehaviourComponent,
        IDamageTakerComponent
    {
        [SerializeField] private HurtBox _hurtBox;
        [SerializeField] private float _invincibilityFrame;
        [FormerlySerializedAs("_damageLayer")] [SerializeField] private FriendLayer friendLayer;
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
            if (damage.Layer == friendLayer)
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
            Debug.Assert(_healthBar != null, "The damage taker must be bound to a health bar", gameObject);
            while (_damageQueue.TryDequeue(out var dmg))
            {
                _healthBar.CurrentHealth -= dmg.BaseAmount;
            }
        }
    }
}