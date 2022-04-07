using System.Collections;
using CombatSystem;
using EntitySystem;
using UnityEngine;

namespace ProjectileSystem
{
    // todo: to:billy find a well-defined convention of handling gameObjects that are components but also have children
    //       (should the children automatically be iterated by Entity.SetUp?
    //       And really: do projectiles need to be a component?
    public class ProjectileComponent : 
        GameObjectComponent,
        IComponent<ProjectileComponent>,
        IRegisterSelf
    {
        [SerializeField] private DamageBox _damageBoxPrefab;
        [SerializeField] private float _damageBoxPersistDuration;
        [SerializeField] private float _damageAmount;
        private DamageLayer _damageLayer;

        private void Awake()
        {
            _damageLayer = DamageLayer.Environment;
        }

        void IRegisterSelf.RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(this);
        }

        public void BindDamageLayer(DamageLayer layer)
        {
            _damageLayer = layer;
        }
        
        public void IgnoreCollisionWith(Collider cld)
        {
            foreach (var selfCld in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(selfCld, cld, true);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Destroyed) return;
            Destroy();
            var damageBox = Instantiate(_damageBoxPrefab, transform.position, Quaternion.identity);
            var damageId = DamageManager.SetDamage(new Damage
            {
                BaseAmount = _damageAmount,
                DamageBox = damageBox,
                Layer = _damageLayer
            });
            
            damageBox.StartCoroutine(DestroyDamageBoxAfterTimeout(_damageBoxPersistDuration));

            IEnumerator DestroyDamageBoxAfterTimeout(float timeOut)
            {
                yield return new WaitForSeconds(timeOut);
                Destroy(damageBox.gameObject);
                DamageManager.ClearDamage(ref damageId);
            }
        }
    }
}