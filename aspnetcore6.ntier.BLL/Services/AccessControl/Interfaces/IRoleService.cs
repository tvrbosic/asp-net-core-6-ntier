using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;

namespace aspnetcore6.ntier.BLL.Services.AccessControl.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetRoles();

        Task<RoleDTO> GetRole(int id);

        Task<bool> AddRole(AddRoleDTO role);

        Task<bool> UpdateRole(UpdateRoleDTO role);

        Task<bool> DeleteRole(int id);
    }
}
