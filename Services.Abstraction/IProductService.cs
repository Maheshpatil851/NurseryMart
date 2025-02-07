using NurseryMart.Contract;

namespace NurseryMart.Services.Abstraction
{
    public interface IProductService : IBaseService
    {
        Task<dynamic> Create(ProductCreateDto entity, CancellationToken cancellationToken);
        Task<dynamic> GetProductsByCategory(int categoryId, CancellationToken cancellationToken);
        Task<dynamic> Search(SearchProductDto entity, CancellationToken cancellationToken);
        Task<dynamic> Update(UpdateProductDto entity, CancellationToken cancellationToken);
        Task<dynamic> GetById(int id, CancellationToken cancellationToken);
    }
}
