using NurseryMart.Entities;
using NurseryMart.IRepository;

namespace NurseryMart.Repositories
{
    internal sealed class CategoryRepository : RepositoryBase<Category>  ,ICategory
    {
        private readonly NurseryMartDbContext _context;
        public CategoryRepository(NurseryMartDbContext context) :base(context)
        {
            _context = context;
        }
    }
}
