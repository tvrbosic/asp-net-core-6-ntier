using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.Shared;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Services.AccessControl
{
    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PermissionDTO>> GetPermissions()
        {
            IEnumerable<Permission> permissions = await _unitOfWork.Permissions.GetAllIncluding(p => p.Department);
            IEnumerable<PermissionDTO> permissionDTOs = _mapper.Map<IEnumerable<PermissionDTO>>(permissions);
            return permissionDTOs;
        }


        public async Task<PaginatedDataDTO<PermissionDTO>> GetPaginatedPermissions(int PageNumber, int PageSize)
        {
            PaginatedData<Permission> paginatedPermissions = await _unitOfWork.Permissions.GetAllPaginated(PageNumber, PageSize);
            PaginatedDataDTO<PermissionDTO> paginatedPermissionDTOs = _mapper.Map<PaginatedDataDTO<PermissionDTO>>(paginatedPermissions);
            return paginatedPermissionDTOs;
        }

        public async Task<PermissionDTO> GetPermission(int id)
        {
            Permission permission = await _unitOfWork.Permissions.GetByIdIncluding(id, p => p.Department);
            PermissionDTO permissionDTO = _mapper.Map<PermissionDTO>(permission);
            return permissionDTO;
        }

        public async Task<bool> AddPermission(AddPermissionDTO permissionDTO)
        {
            Permission permission = _mapper.Map<Permission>(permissionDTO);
            await _unitOfWork.Permissions.Add(permission);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdatePermission(UpdatePermissionDTO permissionDTO)
        {
            Permission permission = _mapper.Map<Permission>(permissionDTO);
            await _unitOfWork.Permissions.Update(permission);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeletePermission(int id)
        {
            await _unitOfWork.Permissions.Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
