using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FinanceDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("FinanceConnection"));
            });
            return services;
        }
    }
}
