using aspnetcore6.ntier.DAL.Models.Abstract;

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
        Department Department { get; set; }
        #endregion

    }
}
