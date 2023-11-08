using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalOffice.Kernel.EFSupport.Extensions;

public static class DatabaseExtension
{
  public static void UpdateDatabase<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
  {
    using IServiceScope serviceScope = app.ApplicationServices
      .GetRequiredService<IServiceScopeFactory>()
      .CreateScope();

    using TDbContext context = serviceScope.ServiceProvider.GetService<TDbContext>();

    context.Database.Migrate();
  }
}
