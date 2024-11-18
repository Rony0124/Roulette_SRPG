using System;
using System.Collections;

namespace TSoft.Utils
{
    public static class MonoUtil
    {
        public static T[] ConvertToArray<T>(this IList list)
        {
            var ret = new T[list.Count];
            list.CopyTo(ret, 0);
            return ret;
        }
        
        public static IList ShuffleList(this IList list)
        {
            var random = new Random();
            for (var i = list.Count - 1; i > 0; i--)
            {
                var randomIndex = random.Next(0, i + 1);
                
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
            return list;
        }
    }
}
