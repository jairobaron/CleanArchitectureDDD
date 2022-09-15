
using CleanArchitectureDDD.Infrastructure.Identity;
using CleanArchitectureDDD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Metrics;
using Serilog;
using App.Metrics.Formatters.Prometheus;

namespace CleanArchitectureDDD.API;

/// <summary>
/// Main Program Class
/// </summary>
public class Program
{
    /// <summary>
    /// Main Function
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public async static Task Main(string[] args)
    {
        var metrics = AppMetrics.CreateDefaultBuilder().OutputMetrics.AsPrometheusPlainText().Build();
        var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

        var host = CreateHostBuilder(args)
                    .ConfigureMetrics(metrics)
                    .UseMetricsEndpoints(options => {
                        options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                        options.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                        options.EnvironmentInfoEndpointEnabled = false;
                    })
                    .UseMetricsWebTracking()
                    .Build();

        using (var scope = host.Services.CreateScope())
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<ConfigDbContextInitialiser>();
            await initialiser.InitialiseAsync();
            await initialiser.SeedAsync();
        }

        try
        {
            //Log.Information("Application Staring Up");
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application failed to start correctly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Create Host Builder with Startup
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>());
}