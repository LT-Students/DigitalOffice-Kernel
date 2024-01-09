using FluentMigrator.Runner;
using FluentMigrator.Runner.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LT.DigitalOffice.Kernel.DapperSupport.Extensions;

public static class DatabaseExtension
{
  public static IHost UpdateDatabase(this IHost host)
  {
    using IServiceScope scope = host.Services.CreateScope();
    IMigrationRunner migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

    try
    {
      migrationService.ListMigrations();
      migrationService.MigrateUp();
    }
    catch (MissingMigrationsException)
    {
      Log.Information("No migrations for dapper was found.");
    }
    catch (SqlException)
    {
      Log.Information("No migrations for dapper was found.");
    }

    return host;
  }
}
