using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Infrastructure.Persistence;
using CleanArchitectureDDD.Infrastructure.Persistence.Interceptors;
using CleanArchitectureDDD.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CleanArchitectureDDD.Infrastructure.Identity;
using CleanArchitectureDDD.Infrastructure.Files;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using CleanArchitectureDDD.Application.Common.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using App.Metrics;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// Ifrastructure Dependency Injection
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Configure services on infrastructure layer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        //Add the service required for using options. 
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        var metrics = new MetricsBuilder()
            .Configuration.Configure(
                options => { options.AddEnvTag(); })
            .OutputMetrics.AsPrometheusPlainText()
            .SampleWith.ForwardDecaying()
            .TimeWith.StopwatchClock().Build();
        services.AddMetrics(metrics);
        services.AddHealthChecks();
        services.AddAppMetricsHealthPublishing();
        services.AddMetricsTrackingMiddleware();
        services.AddMetricsReportingHostedService();
        services.AddMetricsEndpoints(configuration);

        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        })
            .AddMetrics()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ConfigDbContext>(options =>
                options.UseInMemoryDatabase("config_db"));
        }
        else
        {
            services.AddDbContext<ConfigDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(   
                    configuration.GetConnectionString("Default"),                
                    sqlOptions => sqlOptions
                        .MigrationsAssembly(typeof(ConfigDbContext).Assembly.FullName)
                        .EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));
        }       

        services.AddScoped<IConfigDbContext>(provider => provider.GetRequiredService<ConfigDbContext>());

        services.AddScoped<ConfigDbContextInitialiser>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
        services.AddTransient<IExcelFileBuilder, ExcelFileBuilder>();
        services.AddTransient<IExcelFileImport, ExcelFileImport>();
        services.AddTransient<IMetricReporterService, MetricReporterService>();

        //JWT Authorization Configuration
        IConfigurationSection JWTSettingsSection = configuration.GetSection("JWTSettings");
        services.Configure<JWTSettings>(JWTSettingsSection);
        JWTSettings jwtSettings = JWTSettingsSection.Get<JWTSettings>();

        services.AddAuthentication(jwt =>
        {
            jwt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            jwt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JWTKey)),
                ClockSkew = TimeSpan.Zero
            };
            
            config.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        c.NoResult();
                        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        c.Response.ContentType = "application/json";
                        c.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            Code = StatusCodes.Status401Unauthorized,
                            State = "Unauthorized",
                            Message = "Token expired"
                        }));
                    }
                    else
                    {
                        c.NoResult();
                        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        c.Response.ContentType = "application/json";
                        c.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            Code = StatusCodes.Status401Unauthorized,
                            State = "Unauthorized",
                            Message = "Invalid token"
                        }));
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = c =>
                {
                    c.HandleResponse();
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    c.Response.ContentType = "application/json";
                    c.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        State = "Unauthorized",
                        Message = "Authentication required"
                    }));
                    return Task.CompletedTask;
                }
            };            
        });
        
        //Registrar Swagger
        services.AddSwaggerGen(gen =>
        {
            gen.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Clean Architecture DDD API",
                Description = "Clean Architecture DDD related API",                
                Contact = new OpenApiContact()
                {
                    Name = "Jairo Barón",
                    Email = "jairo.baron@gmail.com",
                }
            });
            
            // Agregar token en Swagger
            gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Add Token. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            
            gen.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            if (Assembly.GetEntryAssembly() != null)
                xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            gen.IncludeXmlComments(xmlPath);
        });

        services.AddAuthorization();

        return services;
    }

    private class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object? value)
        {
            if (value == null) { return ""; }

            // Slugify value
            return Regex.Replace((string)value, "([a-z])([A-Z])", "$1_$2").ToLower();
        }
    }
}