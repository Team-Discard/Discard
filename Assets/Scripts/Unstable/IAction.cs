using Unstable.Actions;
using Unstable.Entities;
using Uxt;

namespace Unstable
{
    public interface IAction
    {
        ActionEffects Execute(float deltaTime);

        bool Completed { get; }

        void Begin()
        {
        }

        void Finish()
        {
        }

        void Init(DependencyBag bag)
        {
        }
    }
}