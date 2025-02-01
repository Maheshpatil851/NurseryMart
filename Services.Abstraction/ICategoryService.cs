using NurseryMart.Contract;

namespace NurseryMart.Services.Abstraction
{
    public interface ICategoryService : IBaseService
    {
        Task<dynamic> Create(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken);
        Task<dynamic> GetCategories(Pagination entity, CancellationToken cancellationToken);
    }
}
