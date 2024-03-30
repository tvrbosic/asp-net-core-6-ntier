using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetPermissions();

        Task<PaginatedDataDTO<PermissionDTO>> GetPaginatedPermissions(int PageNumber,
            int PageSize,
            string? searchInput,
            string[]? searchProperties,
            string orderByProperty = "Id",
            bool ascending = true);

        Task<PermissionDTO> GetPermission(int id);

        Task<bool> AddPermission(AddPermissionDTO permission);

        Task<bool> UpdatePermission(UpdatePermissionDTO permission);

        Task<bool> DeletePermission(int id);
    }
}
