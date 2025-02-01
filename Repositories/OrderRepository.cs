using NurseryMart.Entities;
using NurseryMart.IRepository;

namespace NurseryMart.Repositories
{
    internal sealed class OrderRepository : RepositoryBase<Category> ,IOrder
    {
        private readonly NurseryMartDbContext _context;
        public OrderRepository(NurseryMartDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
