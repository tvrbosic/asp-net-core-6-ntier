using aspnetcore6.ntier.Models.AccessControl;

namespace aspnetcore6.ntier.DataAccess.Interfaces.Repositories.AccessControl
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GeByUsername(string username);
    }
}
