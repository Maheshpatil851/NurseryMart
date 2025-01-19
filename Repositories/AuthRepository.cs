using Microsoft.EntityFrameworkCore;
using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Utility;

namespace NurseryMart.Repositories
{
    internal sealed class AuthRepository : RepositoryBase<Authorize>, IAuth
    {
        private readonly NurseryMartDbContext _context;
        public AuthRepository(NurseryMartDbContext context )
            : base(context)
        {
            _context = context;
        }


       

    }
}
