using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.Shared;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Mappings.AccessControl
{
    public class AccessControlProfile : Profile
    {
        public AccessControlProfile()
        {
            // Permission
            CreateMap<Permission, PermissionDTO>();
            CreateMap<PaginatedData<Permission>, PaginatedDataDTO<PermissionDTO>>();
            CreateMap<AddPermissionDTO, Permission>();
            CreateMap<UpdatePermissionDTO, Permission>();

            // Role
            CreateMap<Role, Role>();
            CreateMap<Role, RoleDTO>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.PermissionLinks.Select(pl => pl.Permission)));
            CreateMap<PaginatedData<Role>, PaginatedDataDTO<RoleDTO>>();
            CreateMap<AddRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();

            // User
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleLinks.Select(rl => rl.Role)))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.PermissionLinks.Select(pl => pl.Permission)));
            CreateMap<AddUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();

        }
    }
}
