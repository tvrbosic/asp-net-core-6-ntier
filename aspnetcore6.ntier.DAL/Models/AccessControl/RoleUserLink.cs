#nullable disable
using aspnetcore6.ntier.DAL.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.DAL.Models.AccessControl
{
    public class RoleUserLink : BaseEntity
    {
        #region Navigation
        public int RoleId { get; set; }
        [Required]
        public Role Role { get; set; }
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }
        #endregion
    }
}
