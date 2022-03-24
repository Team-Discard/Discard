using System.Collections.Generic;
using UnityEngine;

namespace Uxt
{
    public static class TransformUtilities
    {
        public static void GetRootComponents<T>(this Transform root, List<T> outputList)
        {
            if (root.TryGetComponent(out T comp))
            {
                outputList.Add(comp);
            }
            else
            {
                GetRootComponentsInChildren(root, outputList);
            }
        }

        public static void GetRootComponentsInChildren<T>(this Transform root, List<T> outputList)
        {
            for (var i = 0; i < root.childCount; ++i)
            {
                GetRootComponents(root.GetChild(i), outputList);
            }
        }
    }
}