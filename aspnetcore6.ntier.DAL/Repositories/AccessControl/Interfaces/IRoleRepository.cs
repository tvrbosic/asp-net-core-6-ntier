using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;

namespace aspnetcore6.ntier.DAL.Repositories.AccessControl.Interfaces
{
    public interface IRoleRepository: IRepository<Role>
    {
        Task AddWithRoles(Role role, List<int> permissionIds);
    }
}
