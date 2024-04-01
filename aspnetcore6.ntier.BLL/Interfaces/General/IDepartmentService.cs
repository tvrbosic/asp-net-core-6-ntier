using aspnetcore6.ntier.BLL.DTOs.General;
using aspnetcore6.ntier.BLL.DTOs.Shared;

namespace aspnetcore6.ntier.BLL.Interfaces.General
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetDepartments();

        PaginatedDataDTO<DepartmentDTO> GetPaginatedDepartments(
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
