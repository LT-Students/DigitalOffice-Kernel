using System;
using System.Net;
using StackExchange.Redis;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers
{
  public static class FlushRedisDbHelper
  {
    public static string FlushDatabase(
      string redisConnStr,
      int database)
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
          }
        }

        return null;
      }
      catch (Exception ex)
      {
        return $"Error while flushing Redis database №{database}. Text: {ex.Message}";
      }
    }
  }
}
