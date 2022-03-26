using System.Collections.Generic;
using EntitySystem;
using JetBrains.Annotations;
using Unstable;
using Unstable.Entities;
using Uxt;

namespace ActionSystem
{
    public interface IActionExecutorComponent : IComponent<IActionExecutorComponent>
    {
        void GetAllActions(List<IAction> outActions);
        bool HasPendingOrActiveActions { get; }
        IReadOnlyFrameData<Translation> TranslationFrame { get; }
        IReadOnlyFrameData<Rotation> RotationFrame { get; }
        void ExecuteAllActions(float deltaTime);
        void AddAction([NotNull] IAction action);
    }
}