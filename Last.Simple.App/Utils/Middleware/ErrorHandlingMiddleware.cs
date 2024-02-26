using System.Text.Json.Serialization;
using System.Text.Json;
using Last.Simple.App.Core.Exceptions;

namespace Last.Simple.App.Utils.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<ErrorHandlingMiddleware> log;

        public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ErrorHandlingMiddleware> log)
        {
            this.next = next;
            this.env = env;
            this.log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = StatusCodes.Status500InternalServerError;
            var message = exception.Message;
            Dictionary<string, IList<string>> errors = new Dictionary<string, IList<string>>();

            switch (exception)
            {
                case EntityNotFoundException:
                    code = StatusCodes.Status404NotFound;
                    break;
                case ArgumentException:                
                case InvalidOperationException:
                    code = StatusCodes.Status400BadRequest;
                    break;                                
                case UnauthorizedAccessException unauthorizedAccessException:
                    code = StatusCodes.Status401Unauthorized;
                    break;
                default:
                    message = exception.ToString();
                    if (!env.IsDevelopment())
                        message = "An unexpected error has occurred in the application.";

                    break;
            }

            log.LogError(exception, exception.Message);

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var errorPayload = new Dictionary<string, object>
            {
                { "code", code },
                { "message", message }
            };

            if (errors.Any())
                errorPayload.Add("errors", errors);

            var result = JsonSerializer.Serialize(errorPayload, jsonOptions);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseHandlingExtension(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
