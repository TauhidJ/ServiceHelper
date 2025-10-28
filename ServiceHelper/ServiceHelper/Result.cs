using ServiceHelper.Dependencies;
//using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = ServiceHelper.Dependencies.Error;
using IResult = ServiceHelper.Dependencies.IResult;

namespace ServiceHelper
{
    [Serializable]

    public struct Result : IResult, ISerializable
    {
        internal static class Messages
        {
            public static readonly string ErrorIsInaccessibleForSuccess = "You attempted to access the Error property for a successful result. A successful result has no Error.";

            public static readonly string ValueIsInaccessibleForFailure = "You attempted to access the Value property for a failed result. A failed result has no Value.";

            public static readonly string ErrorObjectIsNotProvidedForFailure = "You attempted to create a failure result, which must have an error, but a null error object (or empty string) was passed to the constructor.";

            public static readonly string ErrorObjectIsProvidedForSuccess = "You attempted to create a success result, which cannot have an error, but a non-null error object was passed to the constructor.";

            public static readonly string ConvertFailureExceptionOnSuccess = "ConvertFailure failed because the Result is in a success state.";
        }

        public static string ErrorMessagesSeparator = ", ";

        public static bool DefaultConfigureAwait = false;

        private static readonly Func<Exception, string> DefaultTryErrorHandler = (exc) => exc.Message;

        private readonly ResultCommonLogic<Error> _logic;

        public bool IsFailure => _logic.IsFailure;

        public bool IsSuccess => _logic.IsSuccess;

        public Error Error => _logic.Error;

        public static Result Combine(IEnumerable<Result> results, string errorMessagesSeparator = null)
        {
            List<Result> list = results.Where((x) => x.IsFailure).ToList();
            if (list.Count == 0)
            {
                return Success();
            }

            return Failure(string.Join(errorMessagesSeparator ?? ErrorMessagesSeparator, list.Select((x) => x.Error.Message)));
        }

        public static Result Combine<T>(IEnumerable<Result<T>> results, string errorMessagesSeparator = null)
        {
            return Combine(results.Select<Result<T>, Result>((result) => result), errorMessagesSeparator);
        }

        //public static Result<bool, E> Combine<T, E>(IEnumerable<Result<T, E>> results, Func<IEnumerable<E>, E> composerError)
        //{
        //    List<Result<T, E>> list = results.Where<Result<T, E>>((Result<T, E> x) => x.IsFailure).ToList();
        //    if (list.Count == 0)
        //    {
        //        return Success<bool, E>(value: true);
        //    }

        //    return Failure<bool, E>(composerError(list.Select((Result<T, E> x) => x.Error)));
        //}

        //public static Result<bool, E> Combine<T, E>(IEnumerable<Result<T, E>> results) where E : ICombine
        //{
        //    return Combine(results, (IEnumerable<E> errors) => errors.Aggregate((E x, E y) => (E)x.Combine(y)));
        //}

        public static Result Combine(params Result[] results)
        {
            return Combine(results, ErrorMessagesSeparator);
        }

        public static Result Combine<T>(params Result<T>[] results)
        {
            return Combine(results, ErrorMessagesSeparator);
        }

        //public static Result<bool, E> Combine<T, E>(params Result<T, E>[] results) where E : ICombine
        //{
        //    return Combine(results, (IEnumerable<E> errors) => errors.Aggregate((E x, E y) => (E)x.Combine(y)));
        //}

        public static Result Combine(string errorMessagesSeparator, params Result[] results)
        {
            return Combine(results, errorMessagesSeparator);
        }

        public static Result Combine<T>(string errorMessagesSeparator, params Result<T>[] results)
        {
            return Combine(results, errorMessagesSeparator);
        }

        //public static Result<bool, E> Combine<T, E>(Func<IEnumerable<E>, E> composerError, params Result<T, E>[] results)
        //{
        //    return Combine(results, composerError);
        //}

        public Result<K> ConvertFailure<K>()
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException(Messages.ConvertFailureExceptionOnSuccess);
            }

            return Failure<K>(Error);
        }

        public static Result Failure(Error error)
        {
            return new Result(isFailure: true, error);
        }

        public static Result<T> Failure<T>(Error error)
        {
            return new Result<T>(isFailure: true, error, default(T));
        }

        //public static Result<T, E> Failure<T, E>(E error)
        //{
        //    return new Result<T, E>(isFailure: true, error, default(T));
        //}

        //public static Result FailureIf(bool isFailure, string error)
        //{
        //    return SuccessIf(!isFailure, error);
        //}

        //public static Result FailureIf(Func<bool> failurePredicate, string error)
        //{
        //    return SuccessIf(!failurePredicate(), error);
        //}

        //public static async Task<Result> FailureIf(Func<Task<bool>> failurePredicate, string error)
        //{
        //    return SuccessIf(!(await failurePredicate().DefaultAwait()), error);
        //}

        //public static Result<T> FailureIf<T>(bool isFailure, T value, string error)
        //{
        //    return SuccessIf(!isFailure, value, error);
        //}

        //public static Result<T> FailureIf<T>(Func<bool> failurePredicate, T value, string error)
        //{
        //    return SuccessIf(!failurePredicate(), value, error);
        //}

        //public static async Task<Result<T>> FailureIf<T>(Func<Task<bool>> failurePredicate, T value, string error)
        //{
        //    return SuccessIf(!(await failurePredicate().DefaultAwait()), value, error);
        //}

        //public static Result<T, E> FailureIf<T, E>(bool isFailure, T value, E error)
        //{
        //    return SuccessIf(!isFailure, value, error);
        //}

        //public static Result<T, E> FailureIf<T, E>(Func<bool> failurePredicate, T value, E error)
        //{
        //    return SuccessIf(!failurePredicate(), value, error);
        //}

        //public static async Task<Result<T, E>> FailureIf<T, E>(Func<Task<bool>> failurePredicate, T value, E error)
        //{
        //    return SuccessIf(!(await failurePredicate().DefaultAwait()), value, error);
        //}

        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            for (int i = 0; i < results.Length; i++)
            {
                Result result = results[i];
                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Success();
        }

        public static Result Success()
        {
            return new Result(isFailure: false, null);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(isFailure: false, null, value);
        }

        //public static Result<T, E> Success<T, E>(T value)
        //{
        //    return new Result<T, E>(isFailure: false, default(E), value);
        //}

        //public static Result SuccessIf(bool isSuccess, string error)
        //{
        //    if (!isSuccess)
        //    {
        //        return Failure(error);
        //    }

        //    return Success();
        //}

        //public static Result SuccessIf(Func<bool> predicate, string error)
        //{
        //    return SuccessIf(predicate(), error);
        //}

        //public static async Task<Result> SuccessIf(Func<Task<bool>> predicate, string error)
        //{
        //    return SuccessIf(await predicate().DefaultAwait(), error);
        //}

        //public static Result<T> SuccessIf<T>(bool isSuccess, T value, string error)
        //{
        //    if (!isSuccess)
        //    {
        //        return Failure<T>(error);
        //    }

        //    return Success(value);
        //}

        //public static Result<T> SuccessIf<T>(Func<bool> predicate, T value, string error)
        //{
        //    return SuccessIf(predicate(), value, error);
        //}

        //public static async Task<Result<T>> SuccessIf<T>(Func<Task<bool>> predicate, T value, string error)
        //{
        //    return SuccessIf(await predicate().DefaultAwait(), value, error);
        //}

        //public static Result<T, E> SuccessIf<T, E>(bool isSuccess, T value, E error)
        //{
        //    if (!isSuccess)
        //    {
        //        return Failure<T, E>(error);
        //    }

        //    return Success<T, E>(value);
        //}

        //public static Result<T, E> SuccessIf<T, E>(Func<bool> predicate, T value, E error)
        //{
        //    return SuccessIf(predicate(), value, error);
        //}

        //public static async Task<Result<T, E>> SuccessIf<T, E>(Func<Task<bool>> predicate, T value, E error)
        //{
        //    return SuccessIf(await predicate().DefaultAwait(), value, error);
        //}

        public override string ToString()
        {
            if (!IsSuccess)
            {
                return $"Failure({Error})";
            }

            return "Success";
        }

        //public static Result Try(Action action, Func<Exception, string> errorHandler = null)
        //{
        //    errorHandler = errorHandler ?? DefaultTryErrorHandler;
        //    try
        //    {
        //        action();
        //        return Success();
        //    }
        //    catch (Exception arg)
        //    {
        //        return Failure(errorHandler(arg));
        //    }
        //}

        //public static async Task<Result> Try(Func<Task> action, Func<Exception, string> errorHandler = null)
        //{
        //    errorHandler = errorHandler ?? DefaultTryErrorHandler;
        //    try
        //    {
        //        await action().DefaultAwait();
        //        return Success();
        //    }
        //    catch (Exception arg)
        //    {
        //        return Failure(errorHandler(arg));
        //    }
        //}

        //public static Result<T> Try<T>(Func<T> func, Func<Exception, string> errorHandler = null)
        //{
        //    errorHandler = errorHandler ?? DefaultTryErrorHandler;
        //    try
        //    {
        //        return Success(func());
        //    }
        //    catch (Exception arg)
        //    {
        //        return Failure<T>(errorHandler(arg));
        //    }
        //}

        //public static async Task<Result<T>> Try<T>(Func<Task<T>> func, Func<Exception, string> errorHandler = null)
        //{
        //    errorHandler = errorHandler ?? DefaultTryErrorHandler;
        //    try
        //    {
        //        return Success(await func().DefaultAwait());
        //    }
        //    catch (Exception arg)
        //    {
        //        return Failure<T>(errorHandler(arg));
        //    }
        //}

        //public static Result<T, E> Try<T, E>(Func<T> func, Func<Exception, E> errorHandler)
        //{
        //    try
        //    {
        //        return Success<T, E>(func());
        //    }
        //    catch (Exception arg)
        //    {
        //        return Failure<T, E>(errorHandler(arg));
        //    }
        //}

        //public static async Task<Result<T, E>> Try<T, E>(Func<Task<T>> func, Func<Exception, E> errorHandler)
        //{
        //    try
        //    {
        //        return Success<T, E>(await func().DefaultAwait());
        //    }
        //    catch (Exception arg)
        //    {
        //        return Failure<T, E>(errorHandler(arg));
        //    }
        //}

        private Result(bool isFailure, Error error)
        {
            _logic = new ResultCommonLogic<Error>(isFailure, error);
        }

        //private Result(SerializationInfo info, StreamingContext context)
        //{
        //    _logic = ResultCommonLogic<Scrapr.SharedKernel.Types.Result.Error>.Deserialize(info);
        //}

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _logic.GetObjectData(info);
        }
    }
}
