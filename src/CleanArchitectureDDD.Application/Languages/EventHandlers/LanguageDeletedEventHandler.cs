using CleanArchitectureDDD.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDDD.Application.Languages.EventHandlers;

public class LanguageDeletedEventHandler : INotificationHandler<LanguageDeletedEvent>
{
    private readonly ILogger<LanguageDeletedEventHandler> _logger;

    public LanguageDeletedEventHandler(ILogger<LanguageDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LanguageDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CleanArchitectureDDD Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
