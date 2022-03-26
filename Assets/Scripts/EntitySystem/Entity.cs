using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Uxt;

namespace EntitySystem
{
    public static class Entity
    {
        private class ComponentVisitor : IComponentRegistry
        {
            private readonly Action<IComponent> _visitFunc;

            public ComponentVisitor([NotNull] Action<IComponent> visitFunc)
            {
                _visitFunc = visitFunc;
            }

            public void AddComponent(IComponent component) => _visitFunc.Invoke(component);
        }

        public static IComponentRegistry CreateRegistryFromAction([NotNull] Action<IComponent> visitFunc)
        {
            return new ComponentVisitor(visitFunc);
        }

        public static void SetUp(Transform root, IComponentRegistry registry)
        {
            var rcs = new List<IRegisterSelf>();
            root.GetRootComponents(rcs);
            foreach (var rc in rcs)
            {
                var i = rc as IInitialize;
                i?.Init();
                rc.RegisterSelf(registry);
                i?.LateInit();
            }
        }

        public static void SetUp(Transform root, [NotNull] Action<IComponent> visitFunc)
        {
            SetUp(root, CreateRegistryFromAction(visitFunc));
        }
    }
}