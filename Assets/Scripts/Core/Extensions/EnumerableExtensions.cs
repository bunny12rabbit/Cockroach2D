using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || enumerable.IsEmpty();

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            //If this is a list, use the Count property for efficiency. The Count property is O(1) while IEnumerable.Count() is O(N).
            return enumerable is ICollection<T> collection ? collection.Count < 1 : !enumerable.Any();
        }
    }
}