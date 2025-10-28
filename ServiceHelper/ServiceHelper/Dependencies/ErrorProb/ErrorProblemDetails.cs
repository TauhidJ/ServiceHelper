using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ServiceHelper.Dependencies.ErrorProb
{
    public class ErrorProblemDetails<T> : ProblemDetails where T : Error
    {
        public object Error { get; }

        public ErrorProblemDetails(T error, IStringLocalizer localizer, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string? instance = null)
        {
            error.Message = localizer[error.Message];
            base.Type = error.GetType().FullName;
            base.Title = "Error";
            base.Status = (int)statusCode;
            base.Detail = localizer[error.Message];
            base.Instance = instance;
            Error = error;
        }

        public ErrorProblemDetails(T error, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string? instance = null)
        {
            base.Type = error.GetType().FullName;
            base.Title = "Error";
            base.Detail = error.Message;
            base.Status = (int)statusCode;
            base.Instance = instance;
            Error = error;
        }
    }
}
