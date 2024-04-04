#nullable disable
namespace aspnetcore6.ntier.Services.DTO.AccessControl
{
    public class AddRoleDTO
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<int> PermissionIds { get; set; }
    }
}
