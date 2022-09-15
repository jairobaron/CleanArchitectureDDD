using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
