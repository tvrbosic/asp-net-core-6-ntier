using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Mappings.AccessControl
{
    public class AccessControlProfile : Profile
    {
        public AccessControlProfile()
        {
            CreateMap<Permission, PermissionDTO>();
            CreateMap<AddPermissionDTO, Permission>();
            CreateMap<UpdatePermissionDTO, Permission>();
        }
    }
}
