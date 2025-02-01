
using NurseryMart.Entities;
using NurseryMart.IRepository;

namespace NurseryMart.Repositories
{
    public class ProductRepository : RepositoryBase<Product> ,IProduct
    {
        private readonly NurseryMartDbContext _context;
        public ProductRepository(NurseryMartDbContext context) : base(context) 
        {
            _context = context;
        }

    }
}
