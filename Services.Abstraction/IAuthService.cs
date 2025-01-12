namespace NurseryMart.Services.Abstraction
{
    public interface IAuthService
    {
        Task AttachAccountToContext(string token, string client_id, string client_secret, CancellationToken cancellationToken = default);
    }
}
