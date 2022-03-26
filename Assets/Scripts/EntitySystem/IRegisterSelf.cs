using System.Collections.Generic;

namespace EntitySystem
{
    public interface IRegisterSelf
    {
        public void RegisterSelf(IComponentRegistry registry);
    }

    public interface IComponentRegistry
    {
        public void AddComponent(IComponent component);
    }
}