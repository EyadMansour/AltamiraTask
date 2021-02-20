using System.Collections.Generic;

namespace Core.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<TSource> IndexRange<TSource>(
            this IList<TSource> source,
            int fromIndex,
            int toIndex)
        {
            int currIndex = fromIndex;
            while (currIndex <= toIndex)
            {
                yield return source[currIndex];
                currIndex++;
            }
        }
    }
}
