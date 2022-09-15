using CleanArchitectureDDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IConfigDbContext
{
    DbSet<Language> TbMtLanguage { get;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
 