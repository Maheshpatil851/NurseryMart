using NurseryMart.Entities;
using NurseryMart.IRepository;

namespace NurseryMart.Repositories
{
    internal sealed class OrderDetailsRepository :  RepositoryBase<OrderDetails> ,IOrderDetails
    {
        private readonly NurseryMartDbContext _context;
        public OrderDetailsRepository(NurseryMartDbContext context) : base(context) 
        {
            _context = context;

        }
    }
}
