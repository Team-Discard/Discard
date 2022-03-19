using Unstable;

namespace EntitySystem
{
    /// <summary>
    /// A quick and dirty component type to use in prototypes <br/>
    /// **Stable features shall not depend on this type of component.**
    /// </summary>
    public interface IPrototypeComponent : IComponent<IPrototypeComponent>, ITicker
    {
    }
}