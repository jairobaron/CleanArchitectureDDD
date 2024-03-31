namespace CleanArchitectureDDD.Domain.Events;

public class OnStartAppEvent(string appName) : BaseEvent
{
    public string AppName { get; } = appName;
}
