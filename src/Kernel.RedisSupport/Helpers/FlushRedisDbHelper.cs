using System;
using System.Net;
using Serilog;
using StackExchange.Redis;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers
{
  public static class FlushRedisDbHelper
  {
    public static void FlushDatabase(string redisConnStr, int database)
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
            Log.Information($"Redis database {database} successfully flushed.");
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error($"Error while flushing Redis database. Text: {ex.Message}");
      }
    }
  }
}
