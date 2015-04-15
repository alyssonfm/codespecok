using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public static class Utils
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any<T>();
        }

        public static List<T> GetList<T>()
        {
            return new List<T>((T[])Enum.GetValues(typeof(T)));
        }
    }
}