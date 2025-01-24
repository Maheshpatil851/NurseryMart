using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Services.Abstraction;
using NurseryMart.Utility;

namespace NurseryMart.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManager _repositoryManager;
        public ProductService( IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public AccountResponseDto Account { get ; set ; }

        public async Task<dynamic> Create(ProductCreateDto entity ,CancellationToken cancellationToken)
        {
            if (entity == null) throw new RestException(System.Net.HttpStatusCode.BadRequest,ErrorConstant.InvalidInput);
            var newProduct = new Product
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                DiscountPrice = entity.DiscountPrice,
                CategoryId = entity.CategoryId,
                SKU = entity.SKU,
                StockQuantity = entity.StockQuantity,
                Brand = entity.Brand,
                Rating  = entity.Rating,
                IsFeatured  = entity.IsFeatured,
                IsOnSale = entity.IsOnSale,
                ImageUrl = entity.ImageUrl,
                Trail = new Trail { IsActive = true, CreatedBy = Account.Id, CreatedOn = DateTimeOffset.UtcNow }
            };
            await _repositoryManager.ProductRepository.CreateOneAsync(newProduct ,cancellationToken);
            return new ResponseDto(newProduct,1,"product added successfully","success");
        }

        public async Task<dynamic> GetProductsByCategory(int categoryId ,CancellationToken cancellationToken)
        {
            if (categoryId == null) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            var products = await _repositoryManager.ProductRepository.FindManyAsync(_ => _.CategoryId == categoryId && _.Trail.IsActive ,new Pagination { SkipPagination=true},cancellationToken);
            return new ResponseDto(products , products.Count(),"products fetched successfully","success");
        }
    }
}
