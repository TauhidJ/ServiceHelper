namespace ServiceHelper.Dependencies
{
    public interface IDomainEvent
    {
        DateTime CreationDateTime { get; }
    }
}
