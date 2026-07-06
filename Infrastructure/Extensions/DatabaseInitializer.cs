using Infrastructure.Data;
using Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

            await context.Database.MigrateAsync();
            await seeder.SeedAsync();
        }
    }
}
