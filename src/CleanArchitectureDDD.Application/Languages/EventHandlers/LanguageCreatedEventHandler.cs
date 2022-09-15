using CleanArchitectureDDD.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureDDD.Application.Languages.EventHandlers;

public class LanguageCreatedEventHandler : INotificationHandler<LanguageCreatedEvent>
{
    private readonly ILogger<LanguageCreatedEventHandler> _logger;

    public LanguageCreatedEventHandler(ILogger<LanguageCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LanguageCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CleanArchitectureDDD Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
