using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Exceptions;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore6.ntier.BLL.Services.AccessControl
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RoleDTO>> GetRoles()
        {
            IEnumerable<Role> roles = await _unitOfWork.Roles
                .Queryable()
                .Include(r => r.Department)
                .Include(r => r.PermissionLinks)
                .ThenInclude(pl => pl.Permission)
                .ToListAsync();
            IEnumerable<RoleDTO> roleDTOs = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return roleDTOs;
        }

        public PaginatedDataDTO<RoleDTO> GetPaginatedRoles(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true)
        {
            Func<Role, bool>? searchTextPredicate = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchTextPredicate = p => p.Name.Contains(searchText);
            }

            PaginatedData<Role> paginatedRoles = _unitOfWork.Roles.GetAllPaginated(
                PageNumber,
                PageSize,
                searchTextPredicate,
                orderByProperty,
                ascending);
            PaginatedDataDTO<RoleDTO> paginatedRoleDTOs = _mapper.Map<PaginatedDataDTO<RoleDTO>>(paginatedRoles);

            return paginatedRoleDTOs;
        }

        public RoleDTO GetRole(int id)
        {
            Role role = _unitOfWork.Roles
                .Queryable()
                .Include(r => r.Department)
                .Include(r => r.PermissionLinks)
                .ThenInclude(pl => pl.Permission)
                .Single(r => r.Id == id);
            RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);
            return roleDTO;
        }

        public async Task<bool> AddRole(AddRoleDTO roleDTO)
        {
            Role addRole = _mapper.Map<Role>(roleDTO);

            foreach (int permissionId in roleDTO.PermissionIds)
            {
                Permission? permissionToAdd = await _unitOfWork.Permissions.GetById(permissionId);
                if (permissionToAdd != null) { 
                    addRole.PermissionLinks.Add(new PermissionRoleLink
                    {
                        Role = addRole,
                        Permission = permissionToAdd
                    });
                }
            }

            await _unitOfWork.Roles.Add(addRole);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateRole(UpdateRoleDTO roleDTO)
        {
            Role? updateRole = await _unitOfWork.Roles
                .Queryable()
                .Include(r => r.Department)
                .Include(r => r.PermissionLinks)
                .ThenInclude(pl => pl.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleDTO.Id);

            if (updateRole == null)
            {
                throw new EntityNotFoundException($"Update operation failed for entitiy {typeof(Role)} with id: {roleDTO.Id}");
            }

            _mapper.Map(roleDTO, updateRole);

            // Clear previously given permissions
            updateRole.PermissionLinks.Clear();

            foreach (int permissionId in roleDTO.PermissionIds)
            {
                Permission? permissionToAdd = await _unitOfWork.Permissions.GetById(permissionId);
                if (permissionToAdd != null)
                {
                    updateRole.PermissionLinks.Add(new PermissionRoleLink
                    {
                        Role = updateRole,
                        Permission = permissionToAdd
                    });
                }
                
            }
            await _unitOfWork.Roles.Update(updateRole);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteRole(int id)
        {
            await _unitOfWork.Roles.Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
