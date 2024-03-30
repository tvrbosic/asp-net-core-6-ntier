using aspnetcore6.ntier.BLL.DTOs.General;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Models.Shared;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Mappings.General
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDTO>();
            CreateMap<PaginatedData<Department>, PaginatedDataDTO<DepartmentDTO>>();
            CreateMap<AddDepartmentDTO, Department>();
            CreateMap<UpdateDepartmentDTO, Department>();
        }
    }
}
