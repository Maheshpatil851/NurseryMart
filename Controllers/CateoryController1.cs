using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NurseryMart.Contract;
using NurseryMart.Filters;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1", IgnoreApi = false)]
    //[EnableCors("nurseryMart-origins")]
    public class CategoryController :BaseController
    {

        private readonly IServiceManager _servicemanager;
        public CategoryController(IServiceManager serviceManager)
        {
            _servicemanager = serviceManager;
        }

        [HttpPost]
        [AuthorizeFilter]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken)
        {
            _servicemanager.CategoryService.Account = Account;
            var data = await _servicemanager.CategoryService.Create(categoryCreateDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost("get")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetCategories(Pagination entity, CancellationToken cancellationToken)
        {
            _servicemanager.CategoryService.Account = Account;
            var data = await _servicemanager.CategoryService.GetCategories(entity, cancellationToken);
            return Ok(data);
        }


    }
}
