namespace NurseryMart.IRepository
{
    public interface IRepositoryManager
    {
        IOrder OrderRepository { get; }
        IAuth AuthRepository { get; }
        ICategory CategoryRepository { get; }
        IProduct ProductRepository { get; }
        IOrderDetails OrderDetailsRepository { get; }
    }
}
