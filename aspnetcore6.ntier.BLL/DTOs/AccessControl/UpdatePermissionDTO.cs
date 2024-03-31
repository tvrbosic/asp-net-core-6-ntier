#nullable disable
namespace aspnetcore6.ntier.BLL.DTOs.AccessControl
{
    public class UpdatePermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}
