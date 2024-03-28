using aspnetcore6.ntier.DAL.Models.Abstract;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class PermissionRoleLink : BaseEntity
    {
        #region Navigation
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        #endregion
    }
}
