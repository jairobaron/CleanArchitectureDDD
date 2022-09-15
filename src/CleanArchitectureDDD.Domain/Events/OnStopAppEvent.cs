namespace CleanArchitectureDDD.Domain.Events;

public class OnStopAppEvent : BaseEvent
{
    public OnStopAppEvent(string appName)
    {
        AppName = appName;
    }
    public string AppName { get; }
}
