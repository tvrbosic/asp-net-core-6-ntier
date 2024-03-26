using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.General;
using System.Data;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        #region Navigation
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        #endregion

    }
}
