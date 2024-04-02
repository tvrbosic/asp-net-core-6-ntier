#nullable disable
using aspnetcore6.ntier.DAL.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class PermissionRoleLink : BaseEntity
    {
        #region Navigation
        public int PermissionId { get; set; }
        [Required]
        public Permission Permission { get; set; }
        public int RoleId { get; set; }
        [Required]
        public Role Role { get; set; }
        #endregion
    }
}
