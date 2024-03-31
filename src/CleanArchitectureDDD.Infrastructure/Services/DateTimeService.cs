using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
