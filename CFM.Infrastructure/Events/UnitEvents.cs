using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using Prism.Events;

namespace CFM.Infrastructure.Events
{
    public class UnitAddedEvent: PubSubEvent<Unit>
    {
    }

    public class UnitEditedEvent : PubSubEvent<int?>
    {
    }
}
