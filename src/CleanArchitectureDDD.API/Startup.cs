using CleanArchitectureDDD.Application;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Infrastructure;
using CleanArchitectureDDD.Infrastructure.Persistence;
using CleanArchitectureDDD.API.Filters;
using CleanArchitectureDDD.API.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Serilog;

namespace CleanArchitectureDDD.API;
/// <summary>
/// Startup Class
/// </summary>
public class Startup
{
    /// <summary>
    /// Class Constructor
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
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

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ConfigDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

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
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();            
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        //Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        //Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
        //specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(ui =>
        {
            ui.DocumentTitle = "Clean Architecture DDD WebAPI v1";
            ui.RoutePrefix = "swagger";
            ui.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture WebAPI v1");
        });

        app.UseHttpMetrics();
        app.UseMetricServer();
        app.UseMetricsAllMiddleware();
        app.UseSerilogRequestLogging();
        app.UseMetricsAllEndpoints();
        app.UseMetricsApdexTrackingMiddleware();
        app.UseMetricsEndpoint();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "api/v1/config",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
