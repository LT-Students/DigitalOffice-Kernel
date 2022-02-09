using System;
using System.Net;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers
{
  public static class FlushRedisDbHelper
  {
    public static void FlushDatabase(
      string redisConnStr,
      int database,
      ILogger logger = null)
    {
      try
      {
        using (ConnectionMultiplexer cm = ConnectionMultiplexer.Connect(redisConnStr + ",allowAdmin=true,connectRetry=1,connectTimeout=2000"))
        {
          EndPoint[] endpoints = cm.GetEndPoints(true);

          foreach (EndPoint endpoint in endpoints)
          {
            IServer server = cm.GetServer(endpoint);
            server.FlushDatabase(database);
            logger?.LogInformation($"Redis database {database} successfully flushed.");
          }
        }
      }
      catch (Exception ex)
      {
        logger?.LogError($"Error while flushing Redis database. Text: {ex.Message}");
      }
    }
  }
}
