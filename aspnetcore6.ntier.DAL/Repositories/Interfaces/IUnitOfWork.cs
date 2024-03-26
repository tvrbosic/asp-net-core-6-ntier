using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Repositories.AccessControl.Interfaces;

namespace aspnetcore6.ntier.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> Departments { get; }
        IRepository<User> Users { get; }
        IRoleRepository Roles { get; }
        IRepository<Permission> Permissions { get; }

        Task<int> CompleteAsync();
    }
}
