using System.Collections.Generic;
using System.Linq;

namespace MiniJam167.Utility
{
    public static class EnumerableExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            T[] enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            int count = enumerable1.Length;
            int random = UnityEngine.Random.Range(0, count);
            return enumerable1[random];
        }
    }
}