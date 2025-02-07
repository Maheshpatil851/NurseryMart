namespace NurseryMart.MiddleWares
{

    public class LoggingMiddleWare
    {
        private readonly ILogger<LoggingMiddleWare> _logger;
        private readonly RequestDelegate _next;
        public LoggingMiddleWare(RequestDelegate next, ILogger<LoggingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
            var originalBodyStream = context.Response.Body;
            using (var newResponseBody = new System.IO.MemoryStream())
            {
                await _next(context);
                _logger.LogInformation($"Response: {context.Response.StatusCode}");
            }
        }
    }
}
    