using CleanArchitectureDDD.Domain.Entities;
using CleanArchitectureDDD.Domain.ValueObjects;
using CleanArchitectureDDD.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDDD.Infrastructure.Persistence;

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
