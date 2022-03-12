using Uxt;

namespace Unstable.Actions
{
    /// <summary>
    /// A class must implement this method if it can serve
    /// as the source from which to initialize an action (i.e.
    /// it contains the necessary components that a action needs,
    /// such as an animation handler, weapon socket, etc.) <br/>
    ///
    /// This includes any player and enemy controllers from which
    /// an action may be performed. <br/>
    /// </summary>
    public interface IActionInitializer :
        IActionVisitor<Void, IActionInitializer.Init>
    {
        public struct Init
        {
        }

        public sealed bool TryInitialize(IAction action)
        {
            action.Accept(this as IActionVisitor<Void, IActionInitializer.Init>);
            return true;
        }
    }
}