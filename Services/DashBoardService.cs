using NurseryMart.Contract;
using NurseryMart.IRepository;
using NurseryMart.Services.Abstraction;

namespace NurseryMart.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IRepositoryManager _repositoryManager;
        public DashBoardService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public AccountResponseDto Account { get ; set ; }

        public void getdetails()
        {
            
        }
    }
}
