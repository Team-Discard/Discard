using System;
using EntitySystem;
using UnityEngine;

namespace CombatSystem
{
    public class StandardHealthBar : MonoBehaviourComponent, IHealthBarComponent
    {
        [SerializeField] private float _maxHealth;
        public float MaxHealth => _maxHealth; 
        public float CurrentHealth { get; set; }

        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        bool IComponent.EnabledInternal => true;
    }
}