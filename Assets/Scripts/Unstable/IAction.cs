using Unstable.Actions;
using Unstable.Entities;

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

        void Accept(IActionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}