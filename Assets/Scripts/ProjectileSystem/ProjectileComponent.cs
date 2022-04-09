using System;
using System.Collections;
using CombatSystem;
using EntitySystem;
using UnityEngine;

namespace ProjectileSystem
{
    // todo: to:billy find a well-defined convention of handling gameObjects that are components but also have children
    //       (should the children automatically be iterated by Entity.SetUp?
    //       And really: do projectiles need to be a component?
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileComponent :
        GameObjectComponent,
        IComponent<ProjectileComponent>,
        IRegisterSelf
    {
        [SerializeField] private DamageBox _damageBoxPrefab;
        [SerializeField] private float _damageBoxPersistDuration;
        [SerializeField] private float _damageAmount;
        [SerializeField] private float _homingRadius;
        [SerializeField] private float _homingPower;
        private FriendLayer _friendLayer;
        private Rigidbody _rigidbody;

        public bool EnableHoming { get; set; }

        private void Awake()
        {
            _friendLayer = FriendLayer.Environment;
            _rigidbody = GetComponent<Rigidbody>();
            EnableHoming = false;
        }

        void IRegisterSelf.RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(this);
        }

        public void BindDamageLayer(FriendLayer layer)
        {
            _friendLayer = layer;
        }

        public void IgnoreCollisionWith(Collider cld)
        {
            foreach (var selfCld in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(selfCld, cld, true);
            }
        }

        // todo: to:billy The projectile homing system need a refactor
        public void TickHoming(float deltaTime)
        {
            if (!EnableHoming) return;
            var targetColliders = Physics.OverlapSphere(transform.position, _homingRadius,
                LayerMask.GetMask("Projectile Homing Target"),
                QueryTriggerInteraction.Collide);

            ProjectileHomingTarget bestTarget = null;
            var bestDistance = float.MaxValue;

            foreach (var targetCollider in targetColliders)
            {
                if (!targetCollider.TryGetComponent(out ProjectileHomingTarget target)) continue;
                if (_friendLayer == target.FriendLayer) continue;
                var targetDistance = Vector3.Distance(target.transform.position, transform.position);
                if (targetDistance < bestDistance)
                {
                    bestDistance = targetDistance;
                    bestTarget = target;
                }
            }

            if (bestTarget != null)
            {
                Debug.Log($"Best target: {bestTarget.gameObject.name}");
                var currentFwd = transform.forward;
                var targetFwd = bestTarget.transform.position - transform.position;
                var quat = Quaternion.FromToRotation(currentFwd, targetFwd);
                quat.ToAngleAxis(out var angle, out var axis);
                
                // todo: to:billy really, rotation does not affect forward velocity, ahhhhhhhhhhhhhhhhh
                _rigidbody.AddTorque(Mathf.Sign(angle) * _homingPower * axis);
                _rigidbody.velocity = transform.forward * _homingPower;
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
                Layer = _friendLayer
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