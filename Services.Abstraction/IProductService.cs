using NurseryMart.Contract;

namespace NurseryMart.Services.Abstraction
{
    public interface IProductService : IBaseService
    {
        Task<dynamic> Create(ProductCreateDto entity, CancellationToken cancellationToken);
        Task<dynamic> GetProductsByCategory(int categoryId, CancellationToken cancellationToken);
    }
}
