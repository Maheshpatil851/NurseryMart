using Microsoft.EntityFrameworkCore;
using NurseryMart.IRepository;

namespace NurseryMart.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly NurseryMartDbContext _context;
        public RepositoryManager(NurseryMartDbContext context)
        {
            _context = context;
        }
        public IOrder OrderRepository => new OrderRepository(_context);
        public IAuth AuthRepository => new AuthRepository(_context);
        public ICategory CategoryRepository => new CategoryRepository(_context);
        public IProduct ProductRepository => new ProductRepository(_context);
    }
}
