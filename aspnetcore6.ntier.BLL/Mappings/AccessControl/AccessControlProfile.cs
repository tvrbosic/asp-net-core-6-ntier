using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Mappings.AccessControl
{
    public class AccessControlProfile : Profile
    {
        public AccessControlProfile()
        {
            // Permission
            CreateMap<Permission, PermissionDTO>();
            CreateMap<AddPermissionDTO, Permission>();
            CreateMap<UpdatePermissionDTO, Permission>();

            // Role
            CreateMap<Role, Role>();
            CreateMap<Role, RoleDTO>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.PermissionsLink.Select(pl => pl.Permission)));
            CreateMap<AddRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();

            // User
            //CreateMap<User, UserDTO>();
            //CreateMap<AddUserDTO, User>();
            //CreateMap<UpdateUserDTO, User>();

        }
    }
}
