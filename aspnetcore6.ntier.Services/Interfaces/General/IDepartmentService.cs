using aspnetcore6.ntier.Services.DTO.General;
using aspnetcore6.ntier.Services.DTO.Shared;

namespace aspnetcore6.ntier.Services.Interfaces.General
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetDepartments();

        Task<PaginatedDataDTO<DepartmentDTO>> GetPaginatedDepartments(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true);

        Task<DepartmentDTO?> GetDepartment(int id);

        Task AddDepartment(AddDepartmentDTO department);

        Task UpdateDepartment(UpdateDepartmentDTO department);

        Task DeleteDepartment(int id);
    }
}
