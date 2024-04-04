#nullable disable
using aspnetcore6.ntier.Models.Abstract;
using aspnetcore6.ntier.Models.AccessControl;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace aspnetcore6.ntier.Models.General
{
    public class Department : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        #region Navigation
        [JsonIgnore]
        public ICollection<ApplicationUser> Users { get; set; }

        [JsonIgnore]
        public ICollection<Role> Roles { get; set; }

        [JsonIgnore]
        public ICollection<Permission> Permissions { get; set; }
        #endregion
    }
}
