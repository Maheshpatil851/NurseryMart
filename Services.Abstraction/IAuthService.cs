using NurseryMart.Contract;

namespace NurseryMart.Services.Abstraction
{
    public interface IAuthService : IBaseService
    {
        Task AttachAccountToContext(string token, string client_id, string client_secret, CancellationToken cancellationToken = default);
        Task<dynamic> CreateUser(CustomerDTO entity, CancellationToken cancellationToken);
    }
}
