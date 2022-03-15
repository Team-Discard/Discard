using System;
using System.Collections.Generic;

namespace EntitySystem
{
    public class ComponentList<TComponent> : List<TComponent> where TComponent : IComponent
    {
        public void RemoveDestroyed()
        {
            RemoveAll(comp => comp.Destroyed);
        }

        public void Tick(float deltaTime, Action<TComponent, float> tickAction)
        {
            if (tickAction == null)
            {
                return;
            }

            foreach (var comp in this)
            {
                tickAction.Invoke(comp, deltaTime);
            }
        }
    }
}