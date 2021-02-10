using System;
using System.Collections.Generic;
using System.Linq;

namespace DuplicateCodeSearcherLib.Utilities
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Find all indexes duplicate sample row in Enumerable 
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="values">Values</param>
        /// <param name="sampleValue">Values for search</param>
        /// <param name="startIndex">Index from start search</param>
        /// <returns></returns>
        public static List<List<int>> FindAllIndexesOf<T>(this IEnumerable<T> values, T sampleValue, int startIndex)
        {
            return values
                .Select((b, i) => new List<int> { object.Equals(b, sampleValue) ? i : -1 })
                .Where(i => i[0] != -1 && i[0] > startIndex)
                .ToList();
        }
    }
}
