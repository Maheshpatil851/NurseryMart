using Microsoft.IdentityModel.Tokens;
using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;
using NurseryMart.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NurseryMart.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthService(IRepositoryManager repositoryManager,IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public AccountResponseDto Account {  get; set; }
        public async Task AttachAccountToContext(string token, string client_id, string client_secret, CancellationToken cancellationToken = default)
        {
            try
            {

                if (!string.IsNullOrEmpty(token))
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

                    var user = await _repositoryManager.AuthRepository.FindOneAsync(_ => _.Id == id && _.Trail.IsActive == true && !_.Trail.IsMarkedAsDelete);
                    if (user == null)
                        throw new RestException(System.Net.HttpStatusCode.NotFound, ErrorConstant.NotFound);

                    AccountResponseDto responseUser = null;
                    responseUser.Id = user.Id;
                    responseUser.FirstName = user.Profile.FirstName;
                    responseUser.LastName = user.Profile.LastName;
                    responseUser.DateOfBirth = user.Profile.DateOfBirth;
                    responseUser.Gender = user.Profile.Gender;
                    responseUser.IsActive = user.Trail.IsActive;
                   
                    _httpContextAccessor.HttpContext.Items["Account"] = responseUser;

                }

            }
            catch (Exception e)
            {
                throw new RestException(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to access this route.");

            }
        }

        public async Task<dynamic> CreateUser(CustomerDTO entity , CancellationToken cancellationToken)
        {
            var customer = new Authorize
            {
                Email = entity.Email,
                Password = entity.Password,
                State = entity.State,
                Country = entity.Country,
                City = entity.City,
                Profile  = new Profile { FirstName = entity.FirstName , LastName = entity.LastName ,Gender = entity.Gender },
                Trail = new Trail { CreatedOn = DateTimeOffset.UtcNow ,IsActive =true},
            };

            await _repositoryManager.AuthRepository.CreateOneAsync(customer);
            return customer;
        }
    }
}
