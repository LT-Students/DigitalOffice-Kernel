using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Extensions
{
    public static class RedisExtension
    {
        public static string GetRedisCacheHashCode(this IEnumerable<Guid> guids, params object[] additionalArguments)
        {
            unchecked
            {
                int cache = 0;

                foreach (Guid id in guids)
                {
                    cache += id.GetHashCode();
                }

                foreach (Guid arg in additionalArguments)
                {
                    cache += arg.GetHashCode();
                }

                return cache.ToString();
            }
        }

        public static string GetRedisCacheHashCode(this Guid id, params object[] additionalArguments)
        {
            unchecked
            {
                int cache = id.GetHashCode();

                foreach (Guid arg in additionalArguments)
                {
                    cache += arg.GetHashCode();
                }

                return cache.ToString();
            }
        }
    }
}
