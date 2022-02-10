using System.Collections.Generic;
using UnityEngine;
using Unstable.Entities;

namespace Unstable.PlayerActions.Charge
{
    public class ChargeAction : MonoBehaviour, IAction
    {
        [SerializeField] private float _distance;
        [SerializeField] private float _duration;
        private float _speed;

        [SerializeField] private List<DamageVolume> _damageVolumes;

        private PlayerPawn _pawn;
        private Vector2 _direction;

        public bool Completed { get; private set; }

        private void Awake()
        {
            _speed = CalculateSpeed();
            _pawn = null;
            Completed = false;
        }

        private float CalculateSpeed()
        {
            Debug.Assert(_duration > 0.0f);
            return _distance / _duration;
        }

        public ActionEffects Execute(float deltaTime)
        {
            var effects = new ActionEffects();

            if (Completed)
            {
                return effects;
            }

            _duration -= deltaTime;
            if (_duration <= 0.0f)
            {
                Completed = true;
            }

            effects.HorizontalVelocity = _speed * _direction;
            effects.DamageVolumes = _damageVolumes;

            return effects;
        }

        public void Init(PlayerPawn pawn)
        {
            _pawn = pawn;
        }

        public void Begin()
        {
            var fwd = _pawn.transform.forward;
            _direction = new Vector2(fwd.x, fwd.z);
            _direction.Normalize();
        }

        public void Finish()
        {
            Destroy(gameObject);
        }
    }
}