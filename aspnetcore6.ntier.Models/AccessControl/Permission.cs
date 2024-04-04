#nullable disable
using aspnetcore6.ntier.Models.Abstract;
using aspnetcore6.ntier.Models.General;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.Models.AccessControl
{
    public class Permission : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        #region Navigation
        public int DepartmentId { get; set; }
        [Required]
        public Department Department { get; set; }

        public ICollection<PermissionRoleLink> RoleLinks { get; set; } = new List<PermissionRoleLink>();
        #endregion
    }
}
