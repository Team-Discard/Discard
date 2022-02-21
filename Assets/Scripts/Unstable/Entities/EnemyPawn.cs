using System;
using UnityEngine;

namespace Unstable.Entities
{
    public class EnemyPawn : MonoBehaviour, IPawn
    {
        private RigidbodyPawn _internalPawn;

        private void Awake()
        {
            var rigidBody = GetComponent<Rigidbody>();
            _internalPawn = new RigidbodyPawn(rigidBody);
        }

        public void SetTranslationFrame(TranslationFrame translationFrame) =>
            _internalPawn.SetTranslationFrame(translationFrame);

        public RotationFrame GetRotationFrame() => _internalPawn.GetRotationFrame();

        public void SetRotationFrame(RotationFrame rotationFrame) => _internalPawn.SetRotationFrame(rotationFrame);
        public Vector3 CurrentVelocity => _internalPawn.CurrentVelocity;
        public Vector3 CurrentForward => _internalPawn.CurrentForward;

        private void Update()
        {
            _internalPawn.TickRotation(Time.deltaTime);
        }

        private void FixedUpdate() => _internalPawn.TickPhysics(Time.deltaTime);
    }
}