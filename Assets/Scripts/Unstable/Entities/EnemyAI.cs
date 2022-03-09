using System;
using UnityEngine;

namespace Unstable.Entities
{
    public class EnemyAI :
        MonoBehaviour,
        ITicker
    {
        [SerializeField] private float _attackDistance;

        public bool IsSlashing { get; set; }
        public bool IsMoving { get; private set; }
        public Transform PlayerTransform { get; private set; }

        private void Awake()
        {
            PlayerTransform = FindObjectOfType<PlayerPawn>().transform;
            IsMoving = true;
            IsSlashing = false;
        }

        public void Tick(float deltaTime)
        {
            if (IsMoving)
            {
                if (Vector3.Distance(PlayerTransform.position, transform.position) <= _attackDistance)
                {
                    IsMoving = false;
                    IsSlashing = true;
                }
            }
            else if (!IsSlashing)
            {
                IsMoving = true;
            }
        }
    }
}