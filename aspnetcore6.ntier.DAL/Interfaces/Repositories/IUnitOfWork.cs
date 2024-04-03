using aspnetcore6.ntier.Models.AccessControl;
using aspnetcore6.ntier.Models.General;

namespace aspnetcore6.ntier.DataAccess.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> Departments { get; }
        IRepository<ApplicationUser> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Permission> Permissions { get; }

        Task<int> CompleteAsync();
    }
}
