using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.DAL.Models.AccessControl;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
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

        RoleDTO GetRole(int id);

        Task<bool> AddRole(AddRoleDTO role);

        Task<bool> UpdateRole(UpdateRoleDTO role);

        Task<bool> DeleteRole(int id);
    }
}
