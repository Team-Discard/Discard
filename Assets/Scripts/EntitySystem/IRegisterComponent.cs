using System.Collections.Generic;

namespace EntitySystem
{
    public interface IRegisterComponent
    {
        public void RegisterSelf(IComponentRegistry registry);
    }

    public interface IComponentRegistry
    {
        public void AddComponent(IComponent component);
    }
}