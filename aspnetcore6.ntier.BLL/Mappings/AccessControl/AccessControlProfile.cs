using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Models.AccessControl;
using aspnetcore6.ntier.Models.Shared;
using AutoMapper;

namespace aspnetcore6.ntier.Services.Mappings.AccessControl
{
    public class AccessControlProfile : Profile
    {
        public AccessControlProfile()
        {
            // Permission
            CreateMap<Permission, Permission>();
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
            CreateMap<ApplicationUser, ApplicationUser>();
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleLinks.Select(rl => rl.Role)));
            CreateMap<PaginatedData<ApplicationUser>, PaginatedDataDTO<UserDTO>>();
            CreateMap<AddUserDTO, ApplicationUser>();
            CreateMap<UpdateUserDTO, ApplicationUser>();

        }
    }
}
