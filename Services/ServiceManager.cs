using NurseryMart.IRepository;
using NurseryMart.Repositories;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _lazyAuthService;
        private readonly Lazy<IDashBoardService> _lazyDashBoardService;
        private readonly Lazy<IProductService> _lazyProductService;
        private readonly Lazy<IOrderService> _lazyOrderService;
        private readonly Lazy<ICategoryService> _lazyCategoryService;
        public ServiceManager(IRepositoryManager repositoryManager,NurseryMartDbContext context, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(repositoryManager,context, httpContextAccessor, configuration));
            _lazyDashBoardService = new Lazy<IDashBoardService>(() => new DashBoardService(repositoryManager));
            _lazyProductService = new Lazy<IProductService>(() => new ProductService(repositoryManager));
            _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(repositoryManager));
            _lazyCategoryService = new Lazy<ICategoryService>(() => new CategoryService(repositoryManager));
        }

        public IAuthService AuthService => _lazyAuthService.Value;  

        public IDashBoardService DashBoardService => _lazyDashBoardService.Value;

        public IProductService ProductService => _lazyProductService.Value;

        public IOrderService OrderService => _lazyOrderService.Value;

        public ICategoryService CategoryService => _lazyCategoryService.Value;
    }
}
