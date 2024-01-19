using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Messages;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Assist.Lunch._4.Core.Helpers.ExceptionHandler
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    EntityAlreadyExistsException => (int)HttpStatusCode.Conflict,
                    InvalidCredentialsException => (int)HttpStatusCode.Unauthorized,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                //var result = JsonSerializer.Serialize(new { message = error?.Message });
                var result = JsonSerializer.Serialize(MessagesConstructor.ReturnMessage(error?.Message, response.StatusCode));

                await response.WriteAsync(result);
            }
        }
    }
}
