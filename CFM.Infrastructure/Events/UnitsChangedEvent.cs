using CFM.Data.Models;
using Prism.Events;

namespace CFM.Infrastructure.Events
{
    public class UnitsChangedEvent: PubSubEvent<Unit>
    {
    }
}
