using System.Collections.Generic;

namespace Uxt.Utils
{
    public static class EnumerableExt
    {
        public static void ToList<T>(this IEnumerable<T> e, List<T> outputList)
        {
            outputList.Clear();
            foreach (var i in e)
            {
                outputList.Add(i);
            }
        }
    }
}