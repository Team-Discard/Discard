using CombatSystem;
using UnityEngine;
using Unstable;
using Unstable.Entities;

namespace EntitySystem
{
    public class ComponentRegistry : IComponentRegistry
    {
        private static ComponentRegistry _instance;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _instance = new ComponentRegistry();

            Enemies = new ComponentList<IEnemy>();
            HealthBars = new ComponentList<IHealthBar>();
            Pawns = new ComponentList<IPawn>();
            AnimationHandlers = new ComponentList<PawnAnimationHandler>();
        }

        public static void AddEntity(IEntity entity)
        {
            entity.AddTo(_instance);
        }
        
        public static ComponentList<IEnemy> Enemies { get; private set; }

        bool IComponentRegistry.AddEnemy(IEnemy enemy)
        {
            Enemies.Add(enemy);
            return true;
        }

        public static ComponentList<IHealthBar> HealthBars { get; private set; }

        bool IComponentRegistry.AddHealthBar(IHealthBar healthBar)
        {
            HealthBars.Add(healthBar);
            return true;
        }

        public static ComponentList<IPawn> Pawns { get; private set; }

        bool IComponentRegistry.AddPawn(IPawn pawn)
        {
            Pawns.Add(pawn);
            return true;
        }

        public static ComponentList<PawnAnimationHandler> AnimationHandlers { get; private set; }

        bool IComponentRegistry.AddPawnAnimationHandler(PawnAnimationHandler animationHandler)
        {
            AnimationHandlers.Add(animationHandler);
            return true;
        }
    }
}