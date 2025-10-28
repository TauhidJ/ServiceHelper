using MediatR;
using ServiceHelper.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
