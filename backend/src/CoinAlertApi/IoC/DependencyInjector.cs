using CoinAlertApi.Application;
using CoinAlertApi.Application.Interfaces;
using CoinAlertApi.Application.Services;
using CoinAlertApi.Application.Workers;
using CoinAlertApi.Domain.Interfaces;
using CoinAlertApi.Infrastructure.Cache;
using CoinAlertApi.Infrastructure.ExternalApis.CoinGecko;
using CoinAlertApi.Infrastructure.Observability;
using CoinAlertApi.Infrastructure.Persistence;
using CoinAlertApi.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CoinAlertApi.IoC;

public static class DependencyInjector
{
    public static void BindEnvironmentVariables(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));
    }

    public static void RegisterDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName);
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOpportunityRepository, OpportunityRepository>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IOpportunityService, OpportunityService>();
        services.AddSingleton<ICryptoPriceService, CryptoPriceService>();
    }

    public static void RegisterHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("coingecko", c =>
        {
            c.BaseAddress = new Uri("https://api.coingecko.com");
            c.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddSingleton<CoinGeckoPriceClient>();
    }

    public static void RegisterCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"];
            options.InstanceName = "CoinAlert:";
        });

        services.AddSingleton<ICacheService, CacheService>();
    }

    public static void RegisterObservability(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(Telemetry.ServiceName))
            .WithTracing(tracing => tracing
                .AddSource(Telemetry.ServiceName)
                .AddAspNetCoreInstrumentation(opts => opts.RecordException = true)
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(opts => opts.Endpoint = new Uri(configuration["Jaeger:OtlpEndpoint"] ?? "")));
    }

    public static void RegisterSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
    }

    public static void RegisterHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<PriceMonitorService>();
    }
}
