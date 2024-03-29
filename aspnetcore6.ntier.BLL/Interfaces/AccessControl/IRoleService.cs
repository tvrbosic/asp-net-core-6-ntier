using aspnetcore6.ntier.BLL.DTOs.AccessControl;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetRoles();

        RoleDTO GetRole(int id);

        Task<bool> AddRole(AddRoleDTO role);

        Task<bool> UpdateRole(UpdateRoleDTO role);

        Task<bool> DeleteRole(int id);
    }
}
