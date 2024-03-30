using aspnetcore6.ntier.BLL.DTOs.General;
using aspnetcore6.ntier.BLL.DTOs.Shared;

namespace aspnetcore6.ntier.BLL.Interfaces.General
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetDepartments();

        Task<PaginatedDataDTO<DepartmentDTO>> GetPaginatedDepartments(int CurrentPage, int PageSize);

        Task<DepartmentDTO> GetDepartment(int id);

        Task AddDepartment(AddDepartmentDTO department);

        Task UpdateDepartment(UpdateDepartmentDTO department);

        Task DeleteDepartment(int id);
    }
}
