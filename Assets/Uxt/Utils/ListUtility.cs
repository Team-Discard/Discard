using System.Collections.Generic;
using UnityEngine;

namespace Uxt.Utils
{
    public static class ListUtility
    {
        public static void Shuffle<T>(this List<T> list)
        {
            for (var dest = 0; dest < list.Count - 1; ++dest)
            {
                var src = Random.Range(dest + 1, list.Count);
                (list[src], list[dest]) = (list[dest], list[src]);
            }
        }
    }
}