using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.DAL.Models.AccessControl;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetPermissions();

        PaginatedDataDTO<PermissionDTO> GetPaginatedPermissions(int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true);

        Task<PermissionDTO?> GetPermission(int id);

        Task<bool> AddPermission(AddPermissionDTO permission);

        Task<bool> UpdatePermission(UpdatePermissionDTO permission);

        Task<bool> DeletePermission(int id);
    }
}
