#nullable disable
using aspnetcore6.ntier.Services.DTO.General;

namespace aspnetcore6.ntier.Services.DTO.AccessControl
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DepartmentDTO Department { get; set; }
    }
}
