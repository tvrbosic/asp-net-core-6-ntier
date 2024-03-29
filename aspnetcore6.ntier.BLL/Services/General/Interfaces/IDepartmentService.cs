using aspnetcore6.ntier.BLL.Services.General.DTOs;

namespace aspnetcore6.ntier.BLL.Services.General.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetDepartments();

        Task<IEnumerable<DepartmentDTO>> GetPaginatedDepartments(int PageNumber, int PageSize);

        Task<DepartmentDTO> GetDepartment(int id);

        Task AddDepartment(AddDepartmentDTO department);

        Task UpdateDepartment(UpdateDepartmentDTO department);

        Task DeleteDepartment(int id);
    }
}
