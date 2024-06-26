﻿#nullable disable
namespace aspnetcore6.ntier.Services.DTO.AccessControl
{
    public class AddUserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<int> RoleIds { get; set; }
    }
}
