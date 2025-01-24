using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;
using NurseryMart.Utility;

namespace NurseryMart.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CategoryService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public AccountResponseDto Account { get; set; }


        public async Task<dynamic> Create(CategoryCreateDto categoryCreateDto ,CancellationToken cancellationToken)
        {
            if (categoryCreateDto == null) throw new RestException(System.Net.HttpStatusCode.BadRequest,ErrorConstant.InvalidInput);
            var newCategory = new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
                ParentCategoryId = categoryCreateDto.ParentCategoryId,
                Trail = new Trail { IsActive = true, CreatedBy = Account.Id, CreatedOn = DateTimeOffset.UtcNow }
            };
            await _repositoryManager.CategoryRepository.CreateOneAsync(newCategory ,cancellationToken);
            return new ResponseDto(newCategory , 1, "category added successfully" ,"success");
        }

        public async Task<dynamic> GetCategories(Pagination entity, CancellationToken cancellationToken)
        {
            if (entity == null) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            var categories = await _repositoryManager.CategoryRepository.FindManyAsync(_ => _.Trail.IsActive, entity, cancellationToken);
            return new ResponseDto(categories, 1, "category added successfully", "success");
        }

    }
}
