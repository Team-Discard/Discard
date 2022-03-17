using System.Collections.Generic;

namespace EntitySystem
{
    public interface IComponentSource
    {
        public IEnumerable<IComponent> AllComponents { get; }
    }
}