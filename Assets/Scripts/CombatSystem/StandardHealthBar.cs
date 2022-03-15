using UnityEngine;

namespace CombatSystem
{
    public class StandardHealthBar : MonoBehaviour, IHealthBar
    {
        [SerializeField] private float _maxHealth;

        public float MaxHealth => _maxHealth;

        public float CurrentHealth { get; set; }

        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }
    }
}