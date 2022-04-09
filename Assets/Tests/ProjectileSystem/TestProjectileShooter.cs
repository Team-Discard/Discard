using System;
using EntitySystem;
using ProjectileSystem;
using UnityEngine;

namespace Tests.ProjectileSystem
{
    public class TestProjectileShooter : MonoBehaviour
    {
        [SerializeField] private ProjectileComponent _projectilePrefab;
        [SerializeField] private float _shootInterval;
        [SerializeField] private float _shootForce;

        private void Start()
        {
            InvokeRepeating(nameof(ShootProjectile), _shootInterval, _shootInterval);
        }

        private void ShootProjectile()
        {
            var projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
            var projectileTransform = projectile.transform;
            Entity.SetUp(projectileTransform, ComponentRegistry.Instance);
            
            // todo: to:billy setting physics for projectile is annoying. Need an API to do this more easily
            var projectileRigidBody = projectile.GetComponent<Rigidbody>();
            projectile.EnableHoming = true;
            projectileRigidBody.isKinematic = false;
            projectileRigidBody.AddForce(projectileTransform.forward * _shootForce, ForceMode.Impulse);
        }
    }
}