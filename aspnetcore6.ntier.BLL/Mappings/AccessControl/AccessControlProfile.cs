using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;
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
            CreateMap<Role, RoleDTO>();
            CreateMap<AddRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();

            // User
            //CreateMap<User, UserDTO>();
            //CreateMap<AddUserDTO, User>();
            //CreateMap<UpdateUserDTO, User>();

        }
    }
}
