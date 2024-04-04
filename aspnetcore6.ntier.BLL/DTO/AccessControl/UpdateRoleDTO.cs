#nullable disable
namespace aspnetcore6.ntier.Services.DTO.AccessControl
{
    public class UpdateRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<int> PermissionIds { get; set; }
    }
}
