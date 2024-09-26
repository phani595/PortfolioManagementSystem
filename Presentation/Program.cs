using Application.ScheduledServices;
using Application.ServiceRegistration;
using Application.Validations;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.SeedData;
using Infrastructure.ServiceCollectionExtension;
using Presentation.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//Infrastructure Layer Configuration
builder.Services.AddInfrastructureLayer(builder.Configuration);

//Application Layer Configuration
builder.Services.AddApplicationLayer(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<AssetDtoValidator>();


//Hangfire Configuration
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));
builder.Services.AddHangfireServer();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read settings from appsettings.json
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging


var app = builder.Build();


// Configure Hangfire Dashboard
app.UseHangfireDashboard();

// Add Hangfire job to update asset prices daily
RecurringJob.AddOrUpdate<IAssetPriceUpdateService>(
    "update-asset-prices",
    service => service.UpdateAssetPricesAsync(),
    "0 * * * *");

if (app.Environment.IsDevelopment())
{
    // Seed initial data in test environment
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ISeedInitialData>();
    await seeder.SeedData();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Enable Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();