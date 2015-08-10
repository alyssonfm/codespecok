using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons
{
    public static class Utils
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any<T>();
        }

        public static bool IsAnyOfThese(this string data, List<string> these) {
            foreach(string s in these)
            {
                if (data.Equals(s))
                    return true;
            }
            return false;
        }
        public static bool StartWithAnyOfThese(this string data, List<string> these)
        {
            foreach (string s in these)
            {
                if (data.StartsWith(s))
                    return true;
            }
            return false;
        }
        public static bool AreStartOfAnyOfThese(this string data, List<string> these)
        {
            foreach (string s in these)
            {
                if (s.StartsWith(data))
                    return true;
            }
            return false;
        }
        public static List<T> GetList<T>()
        {
            return new List<T>((T[])Enum.GetValues(typeof(T)));
        }
    }
}
