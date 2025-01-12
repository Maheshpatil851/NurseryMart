using NurseryMart.IRepository;

namespace NurseryMart.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IAuth> _lazyAuthRepository;
        private readonly Lazy<IOrder> _lazyOrderRepository;

        public RepositoryManager(SqlConnectionFactory database)
        {
            _lazyAuthRepository = new Lazy<IAuth>(() => new AuthRepository(database));
        }
        public IOrder OrderRepository => throw new NotImplementedException();

        public IAuth AuthRepository => _lazyAuthRepository.Value;
    }
}
