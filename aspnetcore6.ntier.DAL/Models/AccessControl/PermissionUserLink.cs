#nullable disable
using aspnetcore6.ntier.DAL.Models.Abstract;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class PermissionUserLink : BaseEntity
    {
        #region Navigation
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        #endregion
    }
}
