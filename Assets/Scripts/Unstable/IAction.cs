using Unstable.Entities;

namespace Unstable
{
    public interface IAction
    {
        ActionEffects Execute(float deltaTime);

        bool Completed { get; }

        void Init(PlayerPawn pawn);
        
        void Begin()
        {
        }

        void Finish()
        {
        }
    }
}