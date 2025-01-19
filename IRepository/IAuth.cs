using Microsoft.AspNetCore.Components.Forms;
using NurseryMart.Contract;
using NurseryMart.Entities;

namespace NurseryMart.IRepository
{
    public interface IAuth : IBase<Authorize>
    {
        Task<Authorize> CreateUserAsync(AuthorizeDto entity,CancellationToken cancellationToken);
        Task<dynamic> GetUsers(Pagination pagination, CancellationToken cancellationToken);
        Task<dynamic> GetUserDetailsById(int id, CancellationToken cancellationToken);
    }
}
