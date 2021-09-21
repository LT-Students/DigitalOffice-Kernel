using LT.DigitalOffice.Kernel.Constants;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Extensions
{
    public static class IEnumerableExtension
    {
        public static string GetRedisCacheHashCode(this IEnumerable<Guid> guids)
        {
            unchecked
            {
                int cache = 0;

                foreach(Guid id in guids)
                {
                    cache += id.GetHashCode();
                }

                return $"{CachePrefix.Collectrion}{cache}";
            }
        }
    }
}
