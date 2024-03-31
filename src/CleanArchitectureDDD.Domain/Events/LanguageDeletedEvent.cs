namespace CleanArchitectureDDD.Domain.Events;

public class LanguageDeletedEvent(Language item) : BaseEvent
{
    public Language Item { get; } = item;
}
