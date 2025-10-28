namespace ServiceHelper.Dependencies
{
    public interface IResult
    {
        bool IsFailure { get; }

        bool IsSuccess { get; }
    }
}
