namespace ServiceHelper.Dependencies
{
    public interface IValue<out T>
    {
        T Value { get; }
    }
}
