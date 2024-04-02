#nullable disable
using aspnetcore6.ntier.BLL.DTOs.General;

namespace aspnetcore6.ntier.BLL.DTOs.AccessControl
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DepartmentDTO Department { get; set; }
    }
}
