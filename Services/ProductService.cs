using LinqKit;
using Mapster;
using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Services.Abstraction;
using NurseryMart.Utility;
using System.Linq.Expressions;

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
            var productList = new List<Product>();
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
            productList.Add(newProduct);
            await _repositoryManager.ProductRepository.CreateOneAsync(newProduct ,cancellationToken);
            return new ResponseDto(newProduct,1,"product added successfully","success");
        }

        public async Task<dynamic> GetProductsByCategory(int categoryId ,CancellationToken cancellationToken)
        {
            if (categoryId == null) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            var products = await _repositoryManager.ProductRepository.FindManyAsync(_ => _.CategoryId == categoryId && _.Trail.IsActive ,new Pagination { SkipPagination=true},cancellationToken);
            return new ResponseDto(products , products.Count(),"products fetched successfully","success");
        }

        public async Task<dynamic> Search(SearchProductDto entity, CancellationToken cancellationToken)
        {
            if (entity == null)throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            //Expression<Func<Product, bool>> filter = _ => true;
            var filter = PredicateBuilder.New<Product>(p => true);
            if (!string.IsNullOrEmpty(entity.Query))
            {
                filter = filter.And(p => p.Name.Contains(entity.Query) || p.Brand.Contains(entity.Query));
            }
            if (entity.CategoryId != 0 && entity.CategoryId != null)
            {
                filter = filter.And(p => p.CategoryId == entity.CategoryId);
            }
            var products = await _repositoryManager.ProductRepository.FindManyAsync(filter, entity.Pagination.Adapt<Pagination>(), cancellationToken);
            return new ResponseDto(products, products.Count(), "Products fetched successfully", "success");
        }

        public async Task<dynamic> Update(UpdateProductDto entity, CancellationToken cancellationToken)
        {
            if (entity == null)throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            var existingProduct = await _repositoryManager.ProductRepository.FindOneAsync(_ => _.ProductId == entity.Id, cancellationToken);
            if (existingProduct == null)throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");
            existingProduct.Name = entity.Name ?? existingProduct.Name;
            existingProduct.Description = entity.Description ?? existingProduct.Description;
            existingProduct.Price = entity.Price.HasValue ? entity.Price.Value : existingProduct.Price;
            existingProduct.CategoryId = entity.CategoryId.HasValue ? entity.CategoryId.Value : existingProduct.CategoryId;
            existingProduct.Brand = entity.Brand ?? existingProduct.Brand;
            await _repositoryManager.ProductRepository.UpdateOneAsync(existingProduct, cancellationToken);
            return new ResponseDto(existingProduct, 1, "Product updated successfully", "success");
        }

        public async Task<dynamic> GetById(int id ,CancellationToken cancellationToken)
        {
            if (id == null) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            var existingProduct = await _repositoryManager.ProductRepository.FindOneAsync(_ => _.ProductId == id, cancellationToken);
            if (existingProduct == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");
            return new ResponseDto( existingProduct, 1,"product fetched successfully","success");
        }


    }
}
