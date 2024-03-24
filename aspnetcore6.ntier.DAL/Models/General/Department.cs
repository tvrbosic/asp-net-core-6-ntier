using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.AccessControl;

namespace aspnetcore6.ntier.DAL.Models.General
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }

        #region Navigation
        public ICollection<User> Users { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<Permission> Permissions { get; set; }
        #endregion
    }
}
