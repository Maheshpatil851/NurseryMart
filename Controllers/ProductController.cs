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
    public class ProductController : BaseController
    {
        private readonly IServiceManager _servicemanager;
        public ProductController(IServiceManager serviceManager)
        {
            _servicemanager = serviceManager;
        }

        [HttpPost]
        [AuthorizeFilter]
        public async Task<IActionResult> Create(ProductCreateDto entity, CancellationToken cancellationToken)
        {
            _servicemanager.ProductService.Account = Account;
            var data = await _servicemanager.ProductService.Create(entity, cancellationToken);
            return Ok(data);
        }

        [HttpGet("{categoryid}")]
        [AuthorizeFilter]
        public async Task<IActionResult> GetProductsByCategory(string categoryid, CancellationToken cancellationToken)
        {
            _servicemanager.ProductService.Account = Account;
            var data = await _servicemanager.ProductService.GetProductsByCategory(int.Parse(categoryid), cancellationToken);
            return Ok(data);
        }

    }
}
