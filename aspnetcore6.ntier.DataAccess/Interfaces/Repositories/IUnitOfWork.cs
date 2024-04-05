using aspnetcore6.ntier.DataAccess.Repositories.AccessControl;
using aspnetcore6.ntier.Models.AccessControl;
using aspnetcore6.ntier.Models.General;

namespace aspnetcore6.ntier.DataAccess.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> Departments { get; }
        IRepository<Permission> Permissions { get; }
        RoleRepository Roles { get; }
        UserRepository Users { get; }

        Task<int> CompleteAsync();
    }
}
