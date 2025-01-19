using NurseryMart.IRepository;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _lazyAuthService;
        public ServiceManager(IRepositoryManager repositoryManager,NurseryMartDbContext context, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(repositoryManager,context, httpContextAccessor, configuration));
        }

        public IAuthService AuthService => _lazyAuthService.Value;
    }
}
