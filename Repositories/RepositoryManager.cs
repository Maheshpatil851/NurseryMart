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
        public IOrder OrderRepository => throw new NotImplementedException();
        public IAuth AuthRepository => new AuthRepository(_context);
    }
}
