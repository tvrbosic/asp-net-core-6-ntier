using aspnetcore6.ntier.BLL.Services.General.DTOs;

namespace aspnetcore6.ntier.BLL.Services.General.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetDepartments();

        Task<DepartmentDTO> GetDepartment(int id);

        Task<bool> AddDepartment(AddDepartmentDTO department);

        Task<bool> UpdateDepartment(UpdateDepartmentDTO department);

        Task<bool> DeleteDepartment(int id);
    }
}
