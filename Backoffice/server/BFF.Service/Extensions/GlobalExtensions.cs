using System.Collections.Generic;

namespace BFF.Service.Extensions
{
    public static class GlobalExtensions
    {
        public static T GetOrDefault<TSource, T>(
            this Dictionary<TSource, T> dict,
            TSource key,
            T defaultVal = default)
        {
            T obj;
            return dict.TryGetValue(key, out obj) ? obj : defaultVal;
        }
    }
}