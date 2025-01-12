using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;

        public AuthService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Authorize> GetAuthorizeByMobileAsync(string mobile)
        {
            return await _repositoryManager.AuthRepository.FindOneAsync(mobile);
        }
    }
}
