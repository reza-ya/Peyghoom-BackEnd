using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Peyghoom_BackEnd.Exceptions
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            var problemDetails = GetProblemDetials(exception);

            if (problemDetails.Status.HasValue)
            {
                httpContext.Response.StatusCode = problemDetails.Status.Value;
            }
            else
            {
                // TODO: log here
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            _logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);
            // Return false to continue with the default behavior
            // - or - return true to signal that this exception is handled
            return true;
        }

        private static ProblemDetails GetProblemDetials(Exception exception)
        {
            switch (exception)
            {
                case Exception:
                    return new ProblemDetails()
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Title = "TODO",
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                        Detail = "",
                        Extensions = new Dictionary<string, object?>
                                {
                                    { "errors", new[] { exception.Message } } // TODO: delete this message from returning to user
                                },
                    };
                default:
                    return new ProblemDetails()
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Title = "TODO",
                        Detail = "",
                        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                        Extensions = new Dictionary<string, object?>
                                {
                                    { "errors", new[] { "" } }
                                },
                    };
            }
        }

    }
}
