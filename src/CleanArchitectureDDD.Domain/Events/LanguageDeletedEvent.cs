namespace CleanArchitectureDDD.Domain.Events;

public class LanguageDeletedEvent : BaseEvent
{
    public LanguageDeletedEvent(Language item)
    {
        Item = item;
    }

    public Language Item { get; }
}
