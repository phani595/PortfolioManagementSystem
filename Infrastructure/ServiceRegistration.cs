using Application.Interfaces;
using Infrastructure.Repository;
using Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceCollectionExtension
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<PortfolioManagementDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ISeedInitialData, SeedInitialData>();

            // Repositories
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}
