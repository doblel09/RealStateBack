using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Exceptions;
using System.Net;

namespace RealStateApi.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,Exception exception, CancellationToken cancellationToken)
        {
            string exceptionTitle = "Internal server error";
            string detail = exception.Message;
            switch (exception)
            {
                
                case ApiException e:
                    switch (e.ErrorCode)
                    {
                        case (int)HttpStatusCode.BadRequest:
                        exceptionTitle = "Bad request";
                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case (int)HttpStatusCode.InternalServerError:
                            httpContext.Response.StatusCode= (int)HttpStatusCode.InternalServerError; 
                            break;
                        case (int)HttpStatusCode.NotFound:
                            exceptionTitle = "Not found";
                            httpContext.Response.StatusCode=(int)HttpStatusCode.NotFound;
                            break;
                        case (int)HttpStatusCode.NoContent:
                            exceptionTitle = "No content";
                            httpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                            break;
                        default:
                            httpContext.Response.StatusCode =(int)HttpStatusCode.InternalServerError;
                            break;
                    }
                    break;
                case KeyNotFoundException e:
                    exceptionTitle = "Bad request";
                    httpContext.Response.StatusCode = ((int)HttpStatusCode.NotFound);
                   break;
                case ValidationException e:
                    exceptionTitle = "Bad request";
                    detail =((ValidationException)exception).Errors.Aggregate((a,b) => a + ", "+ b);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = exceptionTitle,
                Detail = detail
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails,cancellationToken);

            return true;
        }
    }
}
