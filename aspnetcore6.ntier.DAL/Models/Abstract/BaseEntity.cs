namespace aspnetcore6.ntier.DAL.Models.Abstract
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
            IsDeleted = false;
        }

        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }


    }
}
