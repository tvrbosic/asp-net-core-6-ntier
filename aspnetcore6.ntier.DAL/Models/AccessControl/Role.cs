using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.General;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        #region Navigation
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<RoleUserLink> RoleUserLinks { get; set; } = new List<RoleUserLink>();
        public ICollection<PermissionRoleLink> PermissionsLink { get; set; } = new List<PermissionRoleLink>();
        #endregion

    }
}
