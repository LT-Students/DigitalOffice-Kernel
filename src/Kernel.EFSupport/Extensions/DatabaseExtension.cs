using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LT.DigitalOffice.Kernel.EFSupport.Extensions
{
  public static class DatabaseExtension
  {
    public static void UpdateDatabase<TDbContext>
      (this IApplicationBuilder app)
      where TDbContext : DbContext
    {
     /* string connStr = Environment.GetEnvironmentVariable("ConnectionString");

      _services.AddDbContext<TDbContext>(options =>
      {
        options.UseSqlServer(connStr);
      });*/

      using IServiceScope serviceScope = app.ApplicationServices
          .GetRequiredService<IServiceScopeFactory>()
          .CreateScope();

      using TDbContext context = serviceScope.ServiceProvider.GetService<TDbContext>();

      context.Database.Migrate();
    }
  }
}
