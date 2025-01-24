namespace NurseryMart.Services.Abstraction
{
    public interface IServiceManager
    {
        IAuthService AuthService { get; }
        IDashBoardService DashBoardService { get; }
        IProductService ProductService { get; }
        IOrderService OrderService { get; }
         ICategoryService CategoryService { get; }
    }
}
