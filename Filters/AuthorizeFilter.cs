using Microsoft.AspNetCore.Mvc.Filters;
using NurseryMart.Contract;
using NurseryMart.Utility;

namespace NurseryMart.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilter:Attribute,IAuthorizationFilter
    {
        public AuthorizeFilter()
        {
            
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["Account"] == null)
            { throw new RestException(System.Net.HttpStatusCode.Unauthorized, ErrorConstant.NotAuthorized); }
        }
    }
}
