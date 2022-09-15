using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDDD.Domain.Events;

public class LanguageCreatedEvent : BaseEvent
{
    public LanguageCreatedEvent(Language item)
    {
        Item = item;
    }

    public Language Item { get; }
}
