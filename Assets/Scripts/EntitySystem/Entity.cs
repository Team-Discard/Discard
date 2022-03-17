using System;
using UnityEngine;

namespace EntitySystem
{
    public static class Entity
    {
        public static void SetUp(GameObject root, Action<IComponent> visitComponent)
        {
            var init = root.GetComponentsInChildren<IInitialize>(true);
            var components = root.GetComponentsInChildren<IComponent>(true);
            var src = root.GetComponentsInChildren<IComponentSource>(true);

            foreach (var i in init)
            {
                i.Init();
            }

            if (visitComponent != null)
            {
                foreach (var c in components)
                {
                    visitComponent(c);
                }

                foreach (var s in src)
                {
                    foreach (var c in s.AllComponents)
                    {
                        visitComponent(c);
                    }
                }
            }

            foreach (var i in init)
            {
                i.LateInit();
            }
        }
    }
}