using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NurseryMart.Contract;
using NurseryMart.Services;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1", IgnoreApi = false)]
    public class OrderController : BaseController
    {
        private readonly IServiceManager _servicemanager;
        public OrderController(IServiceManager serviceManager)
        {
            _servicemanager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto entity , CancellationToken cancellationToken)
        {
            _servicemanager.OrderService.Account = Account;
             var data = await _servicemanager.OrderService.CreateOrder(entity , cancellationToken);
            return Ok(data);
        }

        [HttpPost("upload-media")]
        public async Task<IActionResult> UploadMedia(IList<IFormFile> fileMedias, CancellationToken cancellationToken = default)
        {
            _servicemanager.OrderService.Account = Account;
            var url = await _servicemanager.OrderService.UploadMedia(fileMedias, cancellationToken);
            return Ok(url);
        }

        [HttpPost("download")]
        public async Task<IActionResult> Download(string key, CancellationToken cancellationToken = default)
        {
            _servicemanager.OrderService.Account = Account;
            var url = await _servicemanager.OrderService.GetFileFromS3(key, cancellationToken);
            return Ok(url);
        }


    }
}
