using LT.DigitalOffice.Kernel.RedisSupport.Constants;
using Serilog;
using StackExchange.Redis;
using System;
using System.Net;

namespace LT.DigitalOffice.Kernel.RedisSupport.Helpers;

/// <summary>
/// Class for flushing Redis cache databases.
/// </summary>
public static class FlushRedisDbHelper
{
  #region public methods

  /// <summary>
  /// Method for flushing specified cache database.
  /// </summary>
  /// <param name="redisConnStr">Connection string for Redis cache.</param>
  /// <param name="database">ID of database to flush.</param>
  public static void FlushDatabase(
    string redisConnStr,
    Cache database)
  {
    try
    {
      using ConnectionMultiplexer cm = ConnectionMultiplexer.Connect(redisConnStr + ",allowAdmin=true,connectRetry=1,connectTimeout=2000");
      EndPoint[] endpoints = cm.GetEndPoints(true);

      foreach (EndPoint endpoint in endpoints)
      {
        IServer server = cm.GetServer(endpoint);
        server.FlushDatabase((int)database);
      }
    }
    catch (Exception ex)
    {
      Log.Error($"Error while flushing Redis database №{database}. Text: {ex.Message}");
    }
  }

  #endregion
}
