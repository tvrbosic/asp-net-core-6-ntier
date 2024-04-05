using aspnetcore6.ntier.Services.DTO.General;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Models.General;
using aspnetcore6.ntier.Models.Shared;
using AutoMapper;

namespace aspnetcore6.ntier.Services.Mappings.General
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
