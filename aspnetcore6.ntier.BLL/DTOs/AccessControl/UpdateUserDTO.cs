#nullable disable
namespace aspnetcore6.ntier.BLL.DTOs.AccessControl
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<int> RoleIds { get; set; }
    }
}
