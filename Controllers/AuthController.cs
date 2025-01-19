using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NurseryMart.Contract;
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
    public class AuthController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        public AuthController(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync(AuthorizeDto entity ,CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repositoryManager.AuthRepository.CreateUserAsync(entity ,cancellationToken);
                return Ok(user);  // Return created user in response
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);  // Return error if user already exists
            }
        }

        [HttpPost()]
        public async Task<IActionResult> GetUsers(Pagination entity, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _repositoryManager.AuthRepository.GetUsers(entity, cancellationToken);
                return Ok(users);  
            }
            catch (RestException ex)
            {
                return BadRequest(ex.Message);  
            }
        }
    }
}
