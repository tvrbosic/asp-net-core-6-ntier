using aspnetcore6.ntier.BLL.DTOs.AccessControl;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetPermissions();

        Task<PermissionDTO> GetPermission(int id);

        Task<bool> AddPermission(AddPermissionDTO permission);

        Task<bool> UpdatePermission(UpdatePermissionDTO permission);

        Task<bool> DeletePermission(int id);
    }
}
