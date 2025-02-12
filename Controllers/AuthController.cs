using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NurseryMart.Contract;
using NurseryMart.Filters;
using NurseryMart.IRepository;
using NurseryMart.Repositories;
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
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager;
        public AuthController(IRepositoryManager repositoryManager ,IServiceManager serviceManager)
        {
            _repositoryManager = repositoryManager;
            _serviceManager = serviceManager;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync(AuthorizeDto entity ,CancellationToken cancellationToken)
        {
            try
            {
                var user = await _serviceManager.AuthService.CreateUserAsync(entity,cancellationToken);
                return Ok(user);  
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpPost()]
        [AuthorizeFilter]
        public async Task<IActionResult> GetUsers(Pagination entity, CancellationToken cancellationToken)
        {
            try
            {
                _serviceManager.AuthService.Account = Account;
                var users = await _serviceManager.AuthService.GetUsers(entity, cancellationToken);
                return Ok(users);  
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto entity, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _serviceManager.AuthService.Login(entity, cancellationToken);
                return Ok(user);
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetUsers(int id, CancellationToken cancellationToken)
        {
            try
            {
                _serviceManager.AuthService.Account = Account;
                var user = await _serviceManager.AuthService.GetUserDetailsById(id, cancellationToken);
                return Ok(user);
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-new-password")]
        public async Task<IActionResult> CreateNewPassword(CreateNewPasswordDto entity, CancellationToken cancellationToken)
        {
            try
            {
                _serviceManager.AuthService.Account = Account;
                var user = await _serviceManager.AuthService.CreateNewPassword(entity, cancellationToken);
                return Ok(user);
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
