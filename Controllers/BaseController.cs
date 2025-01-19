using Microsoft.AspNetCore.Mvc;
using NurseryMart.Contract;

namespace NurseryMart.Controllers
{
    public class BaseController : ControllerBase
    {
        public AccountResponseDto? Account => HttpContext.Items.ContainsKey("Account") && HttpContext.Items["Account"] != null ? (AccountResponseDto)HttpContext.Items["Account"] : null; //throw new RestException(System.Net.HttpStatusCode.Unauthorized, new { message = "Unauthorized", exception = "", details = "" });

        public string? Token => HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    }
}
