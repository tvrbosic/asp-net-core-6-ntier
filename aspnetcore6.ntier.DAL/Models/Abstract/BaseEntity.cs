using aspnetcore6.ntier.DAL.Models.AccessControl;

namespace aspnetcore6.ntier.DAL.Models.Abstract
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            DateCreated = DateTime.UtcNow;
            DateUpdated = null;
            DateDeleted = null;
            IsDeleted = false;
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated{ get; set; }
        public DateTime? DateDeleted{ get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public int UpdatedById { get; set; }
        public User UpdatedBy { get; set; }

        public int DeletedById { get; set; }
        public User DeletedBy { get; set; }
        #endregion
    }
}