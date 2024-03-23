using aspnetcore6.ntier.DAL.Models.Abstract;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }

        #region Navigation
        public int DepartmentId { get; set; }
        Department Department { get; set; }
        #endregion

    }
}
