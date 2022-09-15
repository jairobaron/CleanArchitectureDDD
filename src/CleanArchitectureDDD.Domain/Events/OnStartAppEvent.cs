namespace CleanArchitectureDDD.Domain.Events;

public class OnStartAppEvent : BaseEvent
{
    public OnStartAppEvent(string appName)
    {
        AppName = appName;
    }
    public string AppName { get; }
}
