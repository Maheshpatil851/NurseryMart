using NurseryMart.Contract;
using System.Net;
using System.Text.Json;

namespace NurseryMart.MiddleWares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;


        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                //if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                //{
                //    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                //    var exceptionMessage = exceptionFeature?.Error?.Message;
                //    await HandleExceptionAsync(context, new RestException(HttpStatusCode.BadRequest, exceptionMessage), _logger, _env);
                //}

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger, _env);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<ErrorHandlingMiddleware> logger,
            IWebHostEnvironment _env
            )
        {
            string? result = null;
            var traceId = Guid.NewGuid();
            switch (exception)
            {

                case RestException re:
                    logger.LogError(re, $"SportzFirst Exception");
                    context.Response.StatusCode = (int)re.Code;
                    if (_env.IsDevelopment())
                    {
                        result = JsonSerializer.Serialize(new
                        {
                            data = Array.Empty<object>(),
                            message = re.Message,
                            exception = re.InnerException != null ? re.InnerException.Message : "",
                            stackTrace = re.StackTrace,
                            traceId
                        });
                    }
                    else
                    {
                        result = JsonSerializer.Serialize(new
                        {
                            data = Array.Empty<object>(),
                            message = re.Message,
                            traceId
                        });
                    }


                    break;
                case Exception e:
                    logger.LogError(e, $"Unhandled Exception");

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    if (_env.IsDevelopment())
                    {
                        result = JsonSerializer.Serialize(new
                        {
                            data = Array.Empty<object>(),
                            message = e.Message,
                            exception = e.InnerException != null ? e.InnerException.Message : "",
                            stackTrace = e.StackTrace
                        });
                    }
                    else
                    {
                        result = JsonSerializer.Serialize(new
                        {
                            data = Array.Empty<object>(),
                            message = e.Message,
                            traceId = traceId
                        });

                    }


                    break;
            }
            logger.LogInformation($"trace ID : {traceId}");
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }
    }
}
