using aspnetcore6.ntier.BLL.Services.General.DTOs;
using aspnetcore6.ntier.DAL.Models.General;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Mappings.General
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDTO>();
            CreateMap<AddDepartmentDTO, Department>();
        }
    }
}
