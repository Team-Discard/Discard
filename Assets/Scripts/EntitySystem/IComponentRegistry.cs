using System;
using CombatSystem;
using UnityEngine;
using Unstable;

namespace EntitySystem
{
    public interface IComponentRegistry
    {
        protected sealed bool ReportComponentNotSupported(Type componentType)
        {
            Debug.LogError($"Trying to add a component of type '{componentType}' that is " +
                           $"not support on a container of type '{GetType()}'");
            return false;
        }

        public bool AddEnemy(IEnemy enemy) => ReportComponentNotSupported(typeof(IEnemy));

        public bool AddDamageTaker(IDamageTaker damageTaker)
        {
            DamageManager.AddDamageTaker(damageTaker);
            return true;
        }
    }
}