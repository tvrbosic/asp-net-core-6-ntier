using aspnetcore6.ntier.BLL.DTOs.General;

namespace aspnetcore6.ntier.BLL.DTOs.AccessControl
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DepartmentDTO Department { get; set; }

        public IEnumerable<PermissionDTO> Permissions { get; set; }
    }
}
