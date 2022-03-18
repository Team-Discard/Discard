using System.Collections.Generic;
using EntitySystem;
using JetBrains.Annotations;
using Unstable;

namespace ActionSystem
{
    public interface IActionExecutor : IComponent<IActionExecutor>
    {
        IReadOnlyList<ActionEffects> Effects { get; }
        void GetAllActions(List<IAction> outActions);
        bool HasPendingOrActiveActions { get; }
        void Execute(float deltaTime);
        void AddAction([NotNull] IAction action);
    }
}