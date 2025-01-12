using NurseryMart.IRepository;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _lazyAuthService;
        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(repositoryManager));
        }

        public IAuthService AuthService => _lazyAuthService.Value;
    }
}
