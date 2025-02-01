using NurseryMart.Contract;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.MiddleWares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {

            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var client_id = context.Request.Headers["client_id"].FirstOrDefault()?.Split(" ").Last();
                var client_secret = context.Request.Headers["client_secret"].FirstOrDefault()?.Split(" ").Last();
                var _serviceManager = context.RequestServices.GetService(typeof(IServiceManager)) as IServiceManager;
                CancellationToken CancellationToken = context?.RequestAborted ?? CancellationToken.None;
                //if (token == null) throw new RestException(System.Net.HttpStatusCode.Unauthorized, new { messae = "Unauthorized", exception = "", details = "" });
                if (_serviceManager != null)
                    await _serviceManager.AuthService.AttachAccountToContext(token, client_id, client_secret, CancellationToken);
            }
            catch (Exception ex)
            {
                throw new RestException(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to access this route.");
            }

            if (context != null)
                await _next(context);
        }



    }






}
