using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Models.AccessControl;

namespace aspnetcore6.ntier.Services.Interfaces.AccessControl
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetPermissions();

        Task<PaginatedDataDTO<PermissionDTO>> GetPaginatedPermissions(int PageNumber,
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
