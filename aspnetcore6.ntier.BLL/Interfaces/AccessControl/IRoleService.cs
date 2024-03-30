﻿using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetRoles();

        Task<PaginatedDataDTO<RoleDTO>> GetPaginatedRoles(int CurrentPage, int PageSize);

        RoleDTO GetRole(int id);

        Task<bool> AddRole(AddRoleDTO role);

        Task<bool> UpdateRole(UpdateRoleDTO role);

        Task<bool> DeleteRole(int id);
    }
}