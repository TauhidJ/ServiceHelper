using System.Runtime.Serialization;

namespace ServiceHelper.Dependencies
{
    internal struct ResultCommonLogic<E>
    {
        private readonly E _error;

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public E Error
        {
            get
            {
                if (!IsFailure)
                {
                    throw new ResultSuccessException();
                }

                return _error;
            }
        }

        public ResultCommonLogic(bool isFailure, E error)
        {
            if (isFailure)
            {
                if (error == null || error is string && error.Equals(string.Empty))
                {
                    throw new ArgumentNullException("error", Result.Messages.ErrorObjectIsNotProvidedForFailure);
                }
            }
            else if (!EqualityComparer<E>.Default.Equals(error, default))
            {
                throw new ArgumentException(Result.Messages.ErrorObjectIsProvidedForSuccess, "error");
            }

            IsFailure = isFailure;
            _error = error;
        }

        public void GetObjectData(SerializationInfo info)
        {
            info.AddValue("IsFailure", IsFailure);
            info.AddValue("IsSuccess", IsSuccess);
            if (IsFailure)
            {
                info.AddValue("Error", Error);
            }
        }

        public void GetObjectData<T>(SerializationInfo info, IValue<T> valueResult)
        {
            GetObjectData(info);
            if (IsSuccess)
            {
                info.AddValue("Value", valueResult.Value);
            }
        }

        public static ResultCommonLogic<E> Deserialize(SerializationInfo info)
        {
            bool boolean = info.GetBoolean("IsFailure");
            E error = boolean ? (E)info.GetValue("Error", typeof(E)) : default;
            return new ResultCommonLogic<E>(boolean, error);
        }
    }
}
