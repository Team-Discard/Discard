using Unstable.Entities;
using Uxt.InterModuleCommunication;

namespace ActionSystem
{
    public interface IAction
    {
        public void Execute(float deltaTime);

        public bool Completed { get; }

        public IReadOnlyFrameData<Translation> TranslationFrame => FrameData<Translation>.NoValue;
        public IReadOnlyFrameData<Rotation> RotationFrame => FrameData<Rotation>.NoValue;
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