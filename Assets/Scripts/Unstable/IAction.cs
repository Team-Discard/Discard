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

        TRet Accept<TRet, TTag>(IActionVisitor<TRet, TTag> visitor)
        {
            return visitor.Visit(this);
        }
    }
}