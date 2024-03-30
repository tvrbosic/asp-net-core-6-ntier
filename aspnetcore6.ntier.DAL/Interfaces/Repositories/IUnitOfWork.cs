using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;

namespace aspnetcore6.ntier.DAL.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> Departments { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Permission> Permissions { get; }

        Task<int> CompleteAsync();
    }
}
