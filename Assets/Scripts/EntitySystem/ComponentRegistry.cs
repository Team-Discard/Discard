using System;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class ComponentRegistry : IComponentRegistry
    {
        private readonly Dictionary<Type, ComponentList> _componentLists = new();

        public ComponentRegistry AllowType<T>() where T : IComponent
        {
            return AllowType(typeof(T));
        }

        public ComponentRegistry AllowType(Type type)
        {
            Debug.Assert(typeof(IComponent).IsAssignableFrom(type));
            if (!_componentLists.ContainsKey(type))
            {
                var list = Activator.CreateInstance(typeof(ComponentList<>).MakeGenericType(type));
                _componentLists.Add(type, (ComponentList)list);
            }

            return this;
        }

        public void AddComponent(IComponent component)
        {
            var type = component.GetComponentType();
            if (!_componentLists.TryGetValue(type, out var list))
            {
                Debug.LogError($"'{type}' is not allowed in this component registry.");
                return;
            }
            list.Add(component);
        }

        public ComponentList<TComponent> Get<TComponent>() where TComponent : IComponent
        {
            var type = typeof(TComponent);
            if (!_componentLists.TryGetValue(type, out var list))
            {
                Debug.LogError($"'{type}' is not allowed in this component registry.");
                return null;
            }

            return (ComponentList<TComponent>)list;
        }
    }
}