using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;

namespace aspnetcore6.ntier.BLL.Services.AccessControl.Interfaces
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
