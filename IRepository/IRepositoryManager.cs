namespace NurseryMart.IRepository
{
    public interface IRepositoryManager
    {
        IOrder OrderRepository { get; }
        IAuth AuthRepository { get; }
    }
}
