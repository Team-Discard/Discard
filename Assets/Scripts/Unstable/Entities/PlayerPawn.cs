using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public class PlayerPawn : MonoBehaviour, IPawn
    {
        private RigidbodyPawn _internalPawn;

        private void Awake()
        {
            var rigidBody = GetComponent<Rigidbody>();
            _internalPawn = new RigidbodyPawn(rigidBody);
        }

        public void SetTranslationFrame(TranslationFrame frame) => _internalPawn.SetTranslationFrame(frame);
        public RotationFrame GetRotationFrame() => _internalPawn.GetRotationFrame();
        public void SetRotationFrame(RotationFrame rotationFrame) => _internalPawn.SetRotationFrame(rotationFrame);

        private void Update()
        {
            _internalPawn.TickRotation(Time.deltaTime);
        }

        private void FixedUpdate() => _internalPawn.TickPhysics(Time.deltaTime);
    }
}