#nullable disable
using aspnetcore6.ntier.DAL.Interfaces.Abstract;
using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace aspnetcore6.ntier.DAL.Models.General
{
    public class Department : BaseEntity, ISoftDeleteProtectedEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsSoftDeleteProtected { get; set; } = true;

        #region Navigation
        [JsonIgnore]
        public ICollection<User> Users { get; set; }

        [JsonIgnore] 
        public ICollection<Role> Roles { get; set; }

        [JsonIgnore]
        public ICollection<Permission> Permissions { get; set; }
        #endregion
    }
}
