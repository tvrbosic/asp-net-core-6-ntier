#nullable disable
using aspnetcore6.ntier.Models.AccessControl;
using aspnetcore6.ntier.Models.General;

namespace aspnetcore6.ntier.Services.DTO.AccessControl
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
