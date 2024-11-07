using System.Collections;

namespace TSoft.Utils
{
    public static class MonoUtil
    {
        public static T[] ConvertToArray<T>(this IList list)
        {
            T[] ret = new T[list.Count];
            list.CopyTo(ret, 0);
            return ret;
        }
    }
}
