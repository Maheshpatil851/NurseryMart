using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;
using NurseryMart.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NurseryMart.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly NurseryMartDbContext _context;


        public AuthService(IRepositoryManager repositoryManager,NurseryMartDbContext context ,IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _context = context;
        }

        public AccountResponseDto Account {  get; set; }
        public async Task AttachAccountToContext(string token, string client_id, string client_secret, CancellationToken cancellationToken = default)
        {
            try
            {

                if (!string.IsNullOrEmpty(token) && token != "null")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();

                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                    var jwtToken = (JwtSecurityToken)validatedToken;


                    var id = jwtToken.Claims.First(x => x.Type == "AuthId").Value;

                    var user = await _context.Authorize.FirstOrDefaultAsync(_ => _.Id == int.Parse(id));
                    if (user == null)
                        throw new RestException(System.Net.HttpStatusCode.NotFound, ErrorConstant.NotFound);

                    AccountResponseDto responseUser = user.Adapt<AccountResponseDto>();
                    _httpContextAccessor.HttpContext.Items["Account"] = responseUser;

                }

            }
            catch (Exception e)
            {
                throw new RestException(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to access this route.");

            }
        }

        public async Task<dynamic> Login(LoginDto entity ,CancellationToken cancellationToken)
        {
            if (entity == null || string.IsNullOrEmpty(entity.Email) || string.IsNullOrEmpty(entity.Password)) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
            var account = await _repositoryManager.AuthRepository.FindOneAsync(_ => _.Email == entity.Email, cancellationToken);
            if(account == null) throw new RestException(System.Net.HttpStatusCode.NotFound, ErrorConstant.NotFound);
            if (account.Password != Helper.HashAnything(entity.Password, Convert.FromBase64String(account.Salt))) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidCredentials);
            var tokenExpiry = DateTime.Now.AddDays(int.Parse(_configuration["Settings:TokenExpiryInDays"]));
            var responseUser = account.Adapt<AccountResponseDto>();
            return new ResponseDto(new {accesstoken = GenerateLoginToken(account.Email,Convert.ToString(account.Id),tokenExpiry) , Account = responseUser },1,"user Fetched successfully","success");
        }

        private string GenerateLoginToken(string email,string id ,DateTime tokenExpiry)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var claims = new List<Claim>
            {
                new Claim("Email", email),
                 new Claim("AuthId", id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<Authorize> CreateUserAsync(AuthorizeDto entity, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Authorize.FirstOrDefaultAsync(u => u.UserName == entity.UserName, cancellationToken);
            if (existingUser != null)
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "User already exists with the same username or email.");
            }
            var salt = Helper.GenerateSalt();
            var user = new Authorize
            {
                UserName = entity.UserName,
                Mobile = entity.Mobile,
                Address = entity.Address,
                Country = entity.Country,
                City = entity.City,
                Pincode = entity.Pincode,
                State = entity.State,
                Salt = Convert.ToBase64String(salt),
                Password = Helper.HashAnything(entity.Password, salt),
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

        public async Task<dynamic> GetUserDetailsById(int id, CancellationToken cancellationToken)
        {
            var user = await _repositoryManager.AuthRepository.FindOneAsync(_ => _.Id == id, cancellationToken);
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
