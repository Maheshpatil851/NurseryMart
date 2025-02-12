using NurseryMart.Contract;
using NurseryMart.Entities;
using System.Threading.Tasks;

namespace NurseryMart.Services.Abstraction
{
    public interface IAuthService : IBaseService
    {
        Task AttachAccountToContext(string token, string client_id, string client_secret, CancellationToken cancellationToken = default);
        Task<Authorize> CreateUserAsync(AuthorizeDto entity, CancellationToken cancellationToken);
        Task<dynamic> GetUsers(Pagination pagination, CancellationToken cancellationToken);
        Task<dynamic> GetUserDetailsById(int id, CancellationToken cancellationToken);
        Task<dynamic> Login(LoginDto entity, CancellationToken cancellationToken);
        Task<dynamic> CreateNewPassword(CreateNewPasswordDto entity, CancellationToken cancellationToken);
    }
}
