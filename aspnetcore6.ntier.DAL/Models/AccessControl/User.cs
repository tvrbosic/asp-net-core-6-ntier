using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.General;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        #region Navigation
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<RoleUserLink> RoleUserLinks { get; set; } = new List<RoleUserLink>();
        #endregion

    }
}
