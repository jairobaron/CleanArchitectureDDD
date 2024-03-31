using CleanArchitectureDDD.Domain.Entities;
using CleanArchitectureDDD.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDDD.Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ConfigDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ConfigDbContextInitialiser
{
    private readonly ILogger<ConfigDbContextInitialiser> _logger;
    private readonly ConfigDbContext _context;

    public ConfigDbContextInitialiser(ILogger<ConfigDbContextInitialiser> logger, ConfigDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        //Defaul Data
        // Seed, if necessary
        if (!_context.TbMtLanguage.Any())
        {
            _context.TbMtLanguage.Add(new Language
            {
                DsLanguage = "English",
                DsPrefix = Prefix.English
            });
            _context.TbMtLanguage.Add(new Language
            {
                DsLanguage = "Español",
                DsPrefix = Prefix.Español
            });

            await _context.SaveChangesAsync();
        }
    }
}
