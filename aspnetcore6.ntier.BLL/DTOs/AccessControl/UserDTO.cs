#nullable disable
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;

namespace aspnetcore6.ntier.BLL.DTOs.AccessControl
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Department Department { get; set; }
        public IEnumerable<RoleDTO> Roles { get; set; }
    }
}
