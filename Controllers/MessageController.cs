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
    public class MessageController : BaseController
    {
        private readonly IServiceManager _servicemanager;
        public MessageController(IServiceManager serviceManager)
        {
            _servicemanager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> SendWhatsapp( CancellationToken cancellationToken)
        {
            _servicemanager.OrderService.Account = Account;
            var data = await _servicemanager.OrderService.SendMessage(cancellationToken);
            return Ok(data);
        }
    }
}
