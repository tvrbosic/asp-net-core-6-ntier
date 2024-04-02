#nullable disable
using aspnetcore6.ntier.DAL.Interfaces.Abstract;
using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.General;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class Permission : BaseEntity, ISoftDeleteProtectedEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsSoftDeleteProtected { get; set; } = true;

        #region Navigation
        public int DepartmentId { get; set; }
        [Required]
        public Department Department { get; set; }

        public ICollection<PermissionRoleLink> RoleLinks { get; set; } = new List<PermissionRoleLink>();
        #endregion
    }
}
