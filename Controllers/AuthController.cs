using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NurseryMart.Contract;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/access/[controller]")]
    [ApiExplorerSettings(GroupName = "v1", IgnoreApi = false)]
    //[EnableCors("nurseryMart-origins")]
    public class AuthController : BaseController
    {
        private readonly IServiceManager _serviceManager;
        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginDto login, CancellationToken cancellationToken)
        //{
        //    var loginRespones = await _serviceManager.AuthService.Login(login, cancellationToken);
        //    return Ok(loginRespones);
        //}
        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount(CustomerDTO accountCreateDto, CancellationToken cancellationToken)
        {
            //_serviceManager.AuthService.Account = Account;
            var account = await _serviceManager.AuthService.CreateUser(accountCreateDto, cancellationToken);
            return Ok(account);
        }
    }
}
