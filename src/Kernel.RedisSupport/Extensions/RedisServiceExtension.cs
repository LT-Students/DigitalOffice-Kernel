using LT.DigitalOffice.Kernel.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;
using System;

namespace DigitalOffice.Kernel.RedisSupport.Extensions;

/// <summary>
/// Class with extension method for working with Redis cache.
/// </summary>
public static class RedisServiceExtension
{
  /// <summary>
  /// Gets redis connection string from <see cref="IConfiguration"/> or <see cref="Environment"/>,
  /// connects to redis database using it and adds a singleton service of <see cref="IConnectionMultiplexer"/>
  /// to the specified <see cref="IServiceCollection"/>.
  /// </summary>
  /// <returns>Redis connection string.</returns>
  public static string AddRedisSingleton(this IServiceCollection services, IConfiguration configuration)
  {
    string redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");
    if (string.IsNullOrEmpty(redisConnectionString))
    {
      redisConnectionString = configuration.GetConnectionString("Redis");

      Log.Information($"Redis connection string from appsettings.json was used. Value '{PasswordHider.Hide(redisConnectionString)}'");
    }
    else
    {
      Log.Information($"Redis connection string from environment was used. Value '{PasswordHider.Hide(redisConnectionString)}'");
    }

    services.AddSingleton<IConnectionMultiplexer>(
      x => ConnectionMultiplexer.Connect(redisConnectionString + ",abortConnect=false,connectRetry=1,connectTimeout=2000"));

    return redisConnectionString;
  }

  /// <summary>
  /// Registers Redis cache provider as distributed cache via <see cref="StackExchangeRedisCacheServiceCollectionExtensions"/>.
  /// </summary>
  /// <remarks>After applying this method Redis can be accessed with <see cref="IDistributedCache"/>.</remarks>
  public static void AddRedisDistributedCache(this IServiceCollection services, IConfiguration configuration)
  {
    string redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");
    if (string.IsNullOrEmpty(redisConnectionString))
    {
      redisConnectionString = configuration.GetConnectionString("Redis");

      Log.Information($"Redis connection string from appsettings.json was used. Value '{PasswordHider.Hide(redisConnectionString)}'");
    }
    else
    {
      Log.Information($"Redis connection string from environment was used. Value '{PasswordHider.Hide(redisConnectionString)}'");
    }

    try
    {
      services.AddStackExchangeRedisCache(options =>
      {
        options.Configuration = redisConnectionString;
        options.InstanceName = "Redis";

        options.ConfigurationOptions = new ConfigurationOptions
        {
          AbortOnConnectFail = false, ConnectRetry = 1, ConnectTimeout = 2000
        };
      });
    }
    catch (Exception e)
    {
      Log.Error(e, "Failed to connect to Redis: {message}", e.Message);
    }
  }
}
