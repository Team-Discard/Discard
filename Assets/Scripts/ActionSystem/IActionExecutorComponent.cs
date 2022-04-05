using System.Collections.Generic;
using EntitySystem;
using JetBrains.Annotations;
using MotionSystem;
using Unstable;
using Unstable.Entities;
using Uxt;
using Uxt.InterModuleCommunication;

namespace ActionSystem
{
    public interface IActionExecutorComponent : IComponent<IActionExecutorComponent>
    {
        public void GetAllActions(List<IAction> outActions);
        public bool HasPendingOrActiveActions { get; }
        public IReadOnlyFrameData<Translation> TranslationFrame { get; }
        public IReadOnlyFrameData<Rotation> RotationFrame { get; }
        public float PlayerControlFactor { get; }
        public void ExecuteAllActions(float deltaTime);
        public void AddAction([NotNull] IAction action);
    }
}