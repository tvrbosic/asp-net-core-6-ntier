using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;
using aspnetcore6.ntier.BLL.Services.AccessControl.Interfaces;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;
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
                .Include(r => r.PermissionsLink)
                .ThenInclude(pl => pl.Permission).ToListAsync();
            IEnumerable<RoleDTO> roleDTOs = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return roleDTOs;
        }

        public RoleDTO GetRole(int id)
        {
            Role role = _unitOfWork.Roles
                .Queryable()
                .Include(r => r.Department)
                .Include(r => r.PermissionsLink)
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
                Permission permissionToAdd = await _unitOfWork.Permissions.GetById(permissionId);
                addRole.PermissionsLink.Add(new PermissionRoleLink
                {
                    Role = addRole,
                    Permission = permissionToAdd
                });
            }

            await _unitOfWork.Roles.Add(addRole);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateRole(UpdateRoleDTO roleDTO)
        {
            Role updateRole = _unitOfWork.Roles
                .Queryable()
                .Include(r => r.Department)
                .Include(r => r.PermissionsLink)
                .ThenInclude(pl => pl.Permission)
                .Single(r => r.Id == roleDTO.Id);


            _mapper.Map(roleDTO, updateRole);

            // Clear previously given permissions
            updateRole.PermissionsLink.Clear();

            foreach (int permissionId in roleDTO.PermissionIds)
            {
                Permission permissionToAdd = await _unitOfWork.Permissions.GetById(permissionId);
                updateRole.PermissionsLink.Add(new PermissionRoleLink
                {
                    Role = updateRole,
                    Permission = permissionToAdd
                });
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
