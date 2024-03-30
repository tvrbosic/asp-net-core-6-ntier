namespace aspnetcore6.ntier.BLL.DTOs.AccessControl
{
    public class AddRoleDTO
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }

        public ICollection<int> PermissionIds { get; set; }
    }
}
