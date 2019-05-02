using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Enumerable擴充類別
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 切割IEnumerable成多個較小的級距
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">資料來源</param>
        /// <param name="chunkSize">固定分割大小</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> enumerable, int chunkSize)
        {
            int itemsReturned = 0;
            var list = enumerable.ToList();
            int count = list.Count;
            while (itemsReturned < count)
            {
                int currentChunkSize = Math.Min(chunkSize, count - itemsReturned);
                yield return list.GetRange(itemsReturned, currentChunkSize);
                itemsReturned += currentChunkSize;
            }
        }
    }
}
