namespace CleanArchitectureDDD.Domain.Events;

public class OnStopAppEvent(string appName) : BaseEvent
{
    public string AppName { get; } = appName;
}
