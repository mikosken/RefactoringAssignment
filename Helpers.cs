using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{
    public static class Helpers
    {
        /// <summary>
        /// Creates a list of length <c>totalItems</c> of randomly ordered items
        /// from <c>allowedItems</c>.
        /// If allowRepeats is true, the same item may appear multiple times.
        /// If allowedItems is empty returns an empty List<T>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="totalItems"></param>
        /// <param name="allowedItems"></param>
        /// <param name="allowRepeats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If repeats are not
        /// allowed and totalItems is greater than number of items in
        /// allowedItems.</exception>
        public static List<T> RandomSelection<T>(int totalItems, HashSet<T> allowedItems, bool allowRepeats = false) {
            if (allowRepeats == false && totalItems > allowedItems.Count)
                throw new ArgumentOutOfRangeException($"totalItems can not be greater than number of elements in allowedItems if repeats are not allowed.");
            if (allowedItems.Count < 1)
                return new List<T>();

            var random = new Random();
            
            // Clone input so we don't accidentally delete elements from original.
            var allowedSet = new HashSet<T>(allowedItems);

            var result = new List<T>();

            for (int i = 0; i < totalItems; i++) {
                var item = allowedSet.ElementAt(random.Next(allowedSet.Count));
                result.Add(item);
                if(allowRepeats == false)
                    allowedSet.Remove(item);
            }
            return result;
        }
    }
}