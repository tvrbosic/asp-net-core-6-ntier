using aspnetcore6.ntier.BLL.Services.General.DTOs;

namespace aspnetcore6.ntier.BLL.Services.AccessControl.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DepartmentDTO Department { get; set; }

        public IEnumerable<PermissionDTO> Permissions { get; set; }
    }
}
