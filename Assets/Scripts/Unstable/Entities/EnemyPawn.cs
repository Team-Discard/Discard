﻿using UnityEngine;

namespace Unstable.Entities
{
    public class EnemyPawn : MonoBehaviour, IPawn
    {
        private CharacterControllerPawn _internalPawn;

        private void Awake()
        {
            var characterController = GetComponent<CharacterController>();
            _internalPawn = new CharacterControllerPawn(characterController);
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