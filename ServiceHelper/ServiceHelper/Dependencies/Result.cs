using System.Runtime.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceHelper.Dependencies
{
    public struct Result<T> : IResult, IValue<T>, ISerializable
    {
        private readonly ResultCommonLogic<Error> _logic;

        private readonly T _value;

        public bool IsFailure => _logic.IsFailure;

        public bool IsSuccess => _logic.IsSuccess;

        public Error Error => _logic.Error;

        public T Value
        {
            get
            {
                if (!IsSuccess)                {
                    throw new ArgumentException(Error.Message);
                }

                return _value;
            }
        }

        public Result ConvertFailure()
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException(Result.Messages.ConvertFailureExceptionOnSuccess);
            }

            return Result.Failure(Error);
        }

        public Result<K> ConvertFailure<K>()
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException(Result.Messages.ConvertFailureExceptionOnSuccess);
            }

            return Result.Failure<K>(Error);
        }

        public override string ToString()
        {
            if (!IsSuccess)
            {
                return $"Failure({Error})";
            }

            return $"Success({Value})";
        }

        internal Result(bool isFailure, Error error, T value)
        {
            _logic = new ResultCommonLogic<Error>(isFailure, error);
            _value = value;
        }

        //private Result(SerializationInfo info, StreamingContext context)
        //{
        //    _logic = ResultCommonLogic<Scrapr.SharedKernel.Types.Result.Error>.Deserialize(info);
        //    _value = (_logic.IsFailure ? default(T) : ((T)info.GetValue("Value", typeof(T))));
        //}

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _logic.GetObjectData(info, this);
        }

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Result.Success();
            }

            return Result.Failure(result.Error);
        }
    }
}
