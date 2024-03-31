#nullable disable
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.DAL.Models.General
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public Guid AuditKey { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Operation { get; set; }
        public DateTime Timestamp { get; set; }
        public string EntityName { get; set; }
        public string EntityData { get; set; }
    }
}
