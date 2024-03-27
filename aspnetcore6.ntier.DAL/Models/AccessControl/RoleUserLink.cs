using aspnetcore6.ntier.DAL.Models.Abstract;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class RoleUserLink : BaseEntity
    {
        #region Navigation
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        #endregion
    }
}
