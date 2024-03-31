using CleanArchitectureDDD.Application;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Infrastructure;
using CleanArchitectureDDD.Infrastructure.Persistence;
using CleanArchitectureDDD.API.Services;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Serilog;

namespace CleanArchitectureDDD.API;
/// <summary>
/// Startup Class
/// </summary>
public class DependencyInjection
{
    /// <summary>
    /// Class Constructor
    /// </summary>
    /// <param name="configuration"></param>
    public DependencyInjection(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configuration Interface for setup of application
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(Configuration);

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<IUser, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ConfigDbContext>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        

        

        

        

        
    }
}
