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
        private Queue<Damage> _damageQueue;

        private void Awake()
        {
            _damageQueue = new Queue<Damage>();
        }

        public bool TryDequeueDamage(out Damage dmg)
        {
            return _damageQueue.TryDequeue(out dmg);
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
        }
    }
}