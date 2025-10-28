using MediatR;

namespace ServiceHelper.DomainEvent
{
    public interface IDomainEvent : INotification
    {
        DateTime CreationDateTime { get; }
    }
}
