using Application.Extensions;
using Application.Mapper;
using Application.ScheduledServices;
using Application.Services;
using Application.Services.User;
using Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace Application.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {

            // App services registration
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAssetService, AssetService>();

            // Add JWT Authentication and Authorization Policies using Extension Methods
            services.AddJwtAuthentication(configuration);
            services.AddAuthorizationPolicies();

            // Add FluentValidation validators
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<AssetDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<PortfolioDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<TransactionDtoValidator>();

            //Auto Mapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddScoped<IAssetPriceUpdateService, AssetPriceUpdateService>();
        }
    }
}
