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

                    //var roleIds = jwtToken.Claims.First(x => x.Type == "RoleIds").Value;
                    var routeValues = ((dynamic)_httpContextAccessor.HttpContext.Request).RouteValues as IReadOnlyDictionary<string, object>;
                    if (routeValues == null)
                        throw new RestException(System.Net.HttpStatusCode.NotFound, ErrorConstant.NotFound);
                    if (!routeValues.ContainsKey("controller"))
                        throw new RestException(System.Net.HttpStatusCode.NotFound, ErrorConstant.NotFound);
                    if (!routeValues.ContainsKey("action"))
                        throw new RestException(System.Net.HttpStatusCode.NotFound, ErrorConstant.NotFound);
                    string controller = routeValues["controller"].ToString();
                    string action = routeValues["action"].ToString();


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

                    //var role = await _repositoryManager.RoleRepository.GetById(user.Role.Id, cancellationToken);
                    //if (role.Operations!=null && role.Operations.Any(_y => (_y.Action == action && _y.Module == controller) || (_y.Action == "*" && _y.Module == controller) || _y.Module == "*"))
                    //{
                    //    context.Items["Account"] = id;
                    //}
                }

            }
            catch (Exception e)
            {
                throw new RestException(System.Net.HttpStatusCode.Unauthorized, "You are not authorized to access this route.");

            }
        }

        //public async Task<Authorize> GetAuthorizeByMobileAsync(string mobile)
        //{
        //    return await _repositoryManager.AuthRepository.FindOneAsync(mobile);
        //}
    }
}
