using ApiClient;
using ApiClient.Authentication;
using ApiClient.Configuration;
using ApiClient.Endpoints;
using ApiClient.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Serilog;

var configBuilder = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json")
     .AddUserSecrets<Program>();

IConfiguration configuration = configBuilder.Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .CreateLogger();

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var bitlogSection = configuration.GetSection(BitlogConfiguration.SectionName);
        services.Configure<BitlogConfiguration>(x => bitlogSection.Bind(x));
        var bitlogConfig = bitlogSection.Get<BitlogConfiguration>();

        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<TokenHandler>();

        services.AddRefitClient<IBitlogEndpoints>()
            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri(bitlogConfig.ApiHost);
            })
            //.AddHttpMessageHandler<HttpLogHandler>()
            .AddHttpMessageHandler<TokenHandler>();

        //services.AddTransient<HttpLogHandler>();

        services.AddHttpClient<TokenService>(x =>
        {
            x.BaseAddress = new Uri(bitlogConfig.TokenHost);
        });

        services.AddScoped<App>();
    })
    .UseSerilog();

using var host = hostBuilder.Build();
using var scope = host.Services.CreateScope();

try
{
    await scope.ServiceProvider
        .GetRequiredService<App>()
        .RunAsync();
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "Application error");
}
