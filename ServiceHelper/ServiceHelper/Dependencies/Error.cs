namespace ServiceHelper.Dependencies
{
    public class Error
    {
        public string Message { get; set; }

        public Error()
        {
        }

        public Error(string message)
        {
            Message = message;
        }

        public static implicit operator Error(string message)
        {
            return new Error(message);
        }

        public static implicit operator string(Error error)
        {
            return error.Message;
        }
    }
}
