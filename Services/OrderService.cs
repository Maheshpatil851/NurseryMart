using NurseryMart.Contract;
using NurseryMart.IRepository;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;
        public OrderService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public AccountResponseDto Account { get ; set ; }
    }
}
