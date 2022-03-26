using Unstable.Entities;
using Uxt;
using Uxt.InterModuleCommunication;

namespace Unstable
{
    public interface IAction
    {
        public void Execute(float deltaTime);

        public bool Completed { get; }

        public IReadOnlyFrameData<Translation> TranslationFrame => FrameData<Translation>.NoValue;
        public IReadOnlyFrameData<Rotation> RotationFrame => FrameData<Rotation>.NoValue;

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