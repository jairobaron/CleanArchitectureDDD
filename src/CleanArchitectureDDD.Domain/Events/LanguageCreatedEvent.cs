namespace CleanArchitectureDDD.Domain.Events;

public class LanguageCreatedEvent(Language item) : BaseEvent
{
    public Language Item { get; } = item;
}
