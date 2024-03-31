using System.Reflection;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Persistence;

public class ConfigDbContext : DbContext, IConfigDbContext
{
    public ConfigDbContext(DbContextOptions<ConfigDbContext> options) : base(options) { }
    
    public DbSet<Language> TbMtLanguage => Set<Language>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());        
    }
}
