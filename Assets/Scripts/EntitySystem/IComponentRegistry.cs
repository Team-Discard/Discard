using System;
using CombatSystem;
using UnityEngine;
using Unstable;
using Unstable.Entities;

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

        public bool AddHealthBar(IHealthBar healthBar) => ReportComponentNotSupported(typeof(IHealthBar));
        public bool AddPawn(IPawn pawn) => ReportComponentNotSupported(typeof(IPawn));
        public bool AddPawnAnimationHandler(PawnAnimationHandler animationHandler) =>
            ReportComponentNotSupported(typeof(PawnAnimationHandler));
    }
}