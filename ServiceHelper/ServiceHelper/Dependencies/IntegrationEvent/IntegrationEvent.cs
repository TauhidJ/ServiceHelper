

namespace ServiceHelper.Dependencies.IntegrationEvent
{
    public class IntegrationEvent
    {
        public Guid Id { get; }

        public DateTime CreationDateTime { get; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDateTime = DateTime.UtcNow;
        }
    }
}
