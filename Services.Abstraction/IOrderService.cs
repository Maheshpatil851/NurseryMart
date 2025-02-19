using NurseryMart.Contract;

namespace NurseryMart.Services.Abstraction
{
    public interface IOrderService : IBaseService
    {
        Task<dynamic> CreateOrder(CreateOrderDto entity, CancellationToken cancellationToken);
        Task<dynamic> SendMessage(CancellationToken cancellationToken);
        Task<dynamic> UploadMedia(IList<IFormFile> fileMedias, CancellationToken cancellationToken = default);
        Task<dynamic> GetFileFromS3(string key, CancellationToken cancellationToken);
    }
}
