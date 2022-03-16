using System.Collections.Generic;

namespace EntitySystem
{
    public static class EnumerableExt
    {
        public static ComponentList<TComponent> ToComponentList<TComponent>(this IEnumerable<TComponent> e)
            where TComponent : IComponent
        {
            var list = new ComponentList<TComponent>();
            foreach (var c in e)
            {
                list.Add(c);
            }

            return list;
        }
    }
}