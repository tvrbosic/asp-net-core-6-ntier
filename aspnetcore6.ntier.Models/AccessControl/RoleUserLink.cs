#nullable disable
using aspnetcore6.ntier.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.Models.AccessControl
{
    public class RoleUserLink : BaseEntity
    {
        #region Navigation
        public int RoleId { get; set; }
        [Required]
        public Role Role { get; set; }
        public int UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        #endregion
    }
}
