using ServiceHelper.Dependencies.ErrorProb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ServiceHelper.Dependencies.ErrorProb
{
    public static class ControllerBaseExtensions
    {
        //
        // Summary:
        //     Returns error problem response.
        //
        // Parameters:
        //   controllerBase:
        //
        //   error:
        //     Error
        //
        // Type parameters:
        //   T:
        //     Error type
        public static ActionResult ErrorProblem<T>(this ControllerBase controllerBase, T error) where T : Error
        {
            return new BadRequestObjectResult(new ErrorProblemDetails<T>(error));
        }

        //
        // Summary:
        //     Returns error problem response.
        //
        // Parameters:
        //   controllerBase:
        //
        //   error:
        //     Error message
        public static ActionResult ErrorProblem(this ControllerBase controllerBase, string error)
        {
            return new BadRequestObjectResult(new ErrorProblemDetails<Error>(new Error(error)));
        }

        //
        // Summary:
        //     Returns error problem response.
        //
        // Parameters:
        //   controllerBase:
        //
        //   error:
        //     Error
        //
        //   localizer:
        //     String localizer
        //
        // Type parameters:
        //   T:
        //     Error type
        public static ActionResult ErrorProblem<T>(this ControllerBase controllerBase, T error, IStringLocalizer localizer) where T : Error
        {
            return new BadRequestObjectResult(new ErrorProblemDetails<T>(error, localizer));
        }
    }
}
