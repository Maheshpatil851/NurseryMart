using NurseryMart.Entities;
using NurseryMart.IRepository;
using NurseryMart.Utility;

namespace NurseryMart.Repositories
{
    internal sealed class AuthRepository : RepositoryBase<Authorize>,IAuth
    {
        private readonly ICollection<Authorize> _auth;
        public AuthRepository(SqlConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
            //_auth = connectionFactory.GetCollection<Authorize>(typeof(Authorize).Name.ToSlugCase());
        }

        // Here, i have to implement any custom methods related to Authorize,
        // or simply use the base repository methods like CreateOneAsync, DeleteOneAsync, etc.

        // Example: Custom method to get an Authorize by Mobile
        //public async Task<Authorize> GetAuthorizeByMobileAsync(string mobile)
        //{
        //    var filter = (Authorize a) => a.Mobile == mobile;
        //    return await FindOneAsync(filter);
        //}
    }
}
