
using MediatR;

namespace ServiceHelper.DomainEvent
{
    public class DomainEvent : IDomainEvent, INotification
    {
        public DateTime CreationDateTime { get; }

        public DomainEvent()
        {
            CreationDateTime = DateTime.UtcNow;
        }
    }
}
