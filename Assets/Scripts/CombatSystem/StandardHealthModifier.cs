using System.Collections.Generic;
using System.Linq;
using EntitySystem;

namespace CombatSystem
{
    public class StandardHealthModifier : 
        StandardComponent, 
        IDamageTakerComponent
    {
        // todo: this class has too many responsibilities. Maybe refactor later?

        private DamageLayer _layer;
        private float _accumulatedDamage = 0;
        private float _invincibilityFrame;
        private List<HurtBox> _hurtBoxes;

        public StandardHealthModifier(DamageLayer damageLayer, float invincibilityFrame,
            List<HurtBox> startingHurtBoxes)
        {
            _layer = damageLayer;
            _invincibilityFrame = invincibilityFrame;
            _hurtBoxes = startingHurtBoxes;
        }

        public StandardHealthModifier(DamageLayer damageLayer, float invincibilityFrame = 0.5f) : this(damageLayer,
            invincibilityFrame, new List<HurtBox>())
        {
        }

        public void SetHurtBoxes(IEnumerable<HurtBox> hurtBoxes)
        {
            _hurtBoxes = hurtBoxes.ToList();
        }

        public void HandleDamage(int id, in Damage damage)
        {
            var dmg = damage;

            if (dmg.Layer == _layer)
            {
                return;
            }

            if (!_hurtBoxes.Any(hb => dmg.DamageBox.CheckOverlap(hb)))
            {
                return;
            }

            if (!DamageManager.SetInvincibilityFrame(this, id, _invincibilityFrame))
            {
                return;
            }

            _accumulatedDamage += dmg.BaseAmount;
        }

        public float ConsumeAllDamage()
        {
            var temp = _accumulatedDamage;
            _accumulatedDamage = 0.0f;
            return temp;
        }

        public void ReckonAllDamage()
        {
        }
    }
}