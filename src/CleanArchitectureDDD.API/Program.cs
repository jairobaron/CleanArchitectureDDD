using CleanArchitectureDDD.Infrastructure.Persistence;
using App.Metrics;
using Serilog;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var metrics = AppMetrics.CreateDefaultBuilder().OutputMetrics.AsPrometheusPlainText().Build();
var configuration = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json")
       .Build();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    //app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
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

app.UseHttpMetrics();
app.UseMetricServer();
app.UseMetricsAllMiddleware();
app.UseSerilogRequestLogging();
app.UseMetricsAllEndpoints();
app.UseMetricsApdexTrackingMiddleware();
app.UseMetricsEndpoint();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "api/v1/config",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

app.Run();

public partial class Program { }