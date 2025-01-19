using Microsoft.EntityFrameworkCore;
using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Utility;
using SharpCompress.Common;

namespace NurseryMart.Repositories
{
    internal sealed class AuthRepository : RepositoryBase<Authorize>, IAuth
    {
        private readonly NurseryMartDbContext _context;
        public AuthRepository(NurseryMartDbContext context)
            : base(context)
        {
            _context = context;
        }


        public async Task<Authorize> CreateUserAsync(AuthorizeDto entity, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Authorize.FirstOrDefaultAsync(u => u.UserName == entity.UserName, cancellationToken);
            if (existingUser != null)
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "User already exists with the same username or email.");
            }

            var user = new Authorize
            {
                UserName = entity.UserName,
                Mobile = entity.Mobile,
                Address = entity.Address,
                Country = entity.Country,
                City = entity.City,
                Pincode = entity.Pincode,
                State = entity.State,

                //PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),  // Encrypt password using BCrypt
                Email = entity.Email,
                Trail = new Trail { CreatedBy = null, CreatedOn = DateTimeOffset.UtcNow }
            };

            await _context.Authorize.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Authorize> FindUserAsync(string username, string email)
        {
            return await _context.Authorize.FirstOrDefaultAsync(u => u.UserName == username || u.Email == email);
        }

        public async Task<dynamic> GetUserDetailsById(int id ,CancellationToken cancellationToken)
        {
           var user = await FindOneAsync(_ => _.Id == id  ,cancellationToken);
           return new { user = new { userName = user.UserName, mobile = user.Mobile, user.City, user.Country, user.State, user.Address } };
        }

        public async Task<dynamic> GetUsers(Pagination pagination, CancellationToken cancellationToken)
        {
            // Initialize the query
            IQueryable<Authorize> query = _context.Authorize;

            // Apply sorting if a SortColumn is provided
            if (!string.IsNullOrEmpty(pagination.SortColumn))
            {
                // This is a simple example of dynamic sorting.
                // You might need to extend this to support more columns and sort directions
                if (pagination.SortDesc)
                {
                    query = query.OrderByDescending(x => EF.Property<object>(x, pagination.SortColumn));
                }
                else
                {
                    query = query.OrderBy(x => EF.Property<object>(x, pagination.SortColumn));
                }
            }

            // Apply pagination if SkipPagination is false
            if (!pagination.SkipPagination)
            {
                // Validate PageNumber and PageSize
                if (pagination.PageNumber <= 0) pagination.PageNumber = 1;
                if (pagination.PageSize <= 0) pagination.PageSize = 10; // You can set a default if not provided

                // Apply Skip and Take based on the page number and size
                query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                             .Take(pagination.PageSize);
            }

            // Fetch the users asynchronously
            var users = await query.ToListAsync(cancellationToken);

            // Get the total count of records for pagination metadata
            var totalCount = await _context.Authorize.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

            // Return the result with pagination metadata
            return new
            {
                Users = users,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

    }
}
