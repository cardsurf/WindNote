using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindNote.Events
{
    class ApplicationExitEvent : PubSubEvent<object>
    {
    }
}
