using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Models.AccessControl;

namespace aspnetcore6.ntier.Services.Interfaces.AccessControl
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetRoles();

        Task<PaginatedDataDTO<RoleDTO>> GetPaginatedRoles(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true);

        Task<RoleDTO> GetRole(int id);

        Task<bool> AddRole(AddRoleDTO role);

        Task<bool> UpdateRole(UpdateRoleDTO role);

        Task<bool> DeleteRole(int id);
    }
}
