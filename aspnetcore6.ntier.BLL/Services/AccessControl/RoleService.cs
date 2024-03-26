using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;
using aspnetcore6.ntier.BLL.Services.AccessControl.Interfaces;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;
using AutoMapper;

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
            IEnumerable<Role> roles = await _unitOfWork.Roles.GetAllIncluding(p => p.Department, p => p.Permissions);
            IEnumerable<RoleDTO> roleDTOs = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return roleDTOs;
        }

        public async Task<RoleDTO> GetRole(int id)
        {
            Role role = await _unitOfWork.Roles.GetByIdIncluding(id, p => p.Department, p => p.Permissions);
            RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);
            return roleDTO;
        }

        public async Task<bool> AddRole(AddRoleDTO roleDTO)
        {
            try
            {
                Role role = _mapper.Map<Role>(roleDTO);

                foreach (int permissionId in roleDTO.PermissionIds)
                {
                    Permission permissionToAdd = await _unitOfWork.Permissions.GetById(permissionId);
                    role.Permissions.Add(permissionToAdd);
                }

                await _unitOfWork.Roles.Add(role);
                return await _unitOfWork.CompleteAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                // TODO: Log error
                return false;
            }
        }

        public async Task<bool> UpdateRole(UpdateRoleDTO roleDTO)
        {
            try
            {
                Role roleToUpdate = await _unitOfWork.Roles.GetByIdIncluding(roleDTO.Id, r => r.Permissions);
                _mapper.Map(roleDTO, roleToUpdate);

                // Clear previously given permissions
                roleToUpdate.Permissions.Clear();

                foreach (int permissionId in roleDTO.PermissionIds)
                {
                    Permission permissionToAdd = await _unitOfWork.Permissions.GetById(permissionId);
                    roleToUpdate.Permissions.Add(permissionToAdd);
                }
                _unitOfWork.Roles.Update(roleToUpdate);
                return await _unitOfWork.CompleteAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                // TODO: Log error
                return false;
            }
        }

        public async Task<bool> DeleteRole(int id)
        {
            try
            {
                await _unitOfWork.Roles.Delete(id);
                return await _unitOfWork.CompleteAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                // TODO: Log error
                return false;
            }
        }
    }
}
