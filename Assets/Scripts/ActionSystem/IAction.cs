using MotionSystem;
using Unstable.Entities;
using Uxt.InterModuleCommunication;

namespace ActionSystem
{
    public interface IAction
    {
        public void Execute(float deltaTime);

        public bool Completed { get; }

        public IReadOnlyFrameData<Translation> TranslationFrame => FrameData<Translation>.Identity;
        public IReadOnlyFrameData<Rotation> RotationFrame => FrameData<Rotation>.Identity;
        public float PlayerControlFactor => 0.0f;

        public void Begin()
        {
        }

        public void Finish()
        {
        }

        public void Init(DependencyBag bag)
        {
        }
    }
}