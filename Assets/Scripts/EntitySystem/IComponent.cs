using System;
using UnityEngine;

namespace EntitySystem
{
    public interface IComponent
    {
        public sealed bool Enabled => !Destroyed && EnabledInternal;
        protected bool EnabledInternal => true;
        public bool Destroyed { get; }
        public void Destroy();
        public Type GetComponentType();

        public sealed bool IsComponentOfType<T>(out T component) where T : IComponent
        {
            if (typeof(T) != GetComponentType())
            {
                component = default;
                return false;
            }

            component = (T)this;
            return true;
        }

        public sealed bool IsComponentOfType<T>() where T : IComponent
        {
            return IsComponentOfType(out T _);
        }
    }

    public interface IComponent<T> : IComponent where T : IComponent
    {
        Type IComponent.GetComponentType() => typeof(T);
    }
}