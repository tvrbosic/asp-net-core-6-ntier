using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.General;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        #region Navigation
        public int DepartmentId { get; set; }
        [Required]
        public Department Department { get; set; }

        public ICollection<RoleUserLink> UserLinks { get; set; } = new List<RoleUserLink>();
        public ICollection<PermissionRoleLink> PermissionLinks { get; set; } = new List<PermissionRoleLink>();
        #endregion

    }
}
