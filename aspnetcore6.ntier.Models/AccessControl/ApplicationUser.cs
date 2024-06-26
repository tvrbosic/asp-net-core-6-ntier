﻿#nullable disable
using aspnetcore6.ntier.Models.Abstract;
using aspnetcore6.ntier.Models.General;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore6.ntier.Models.AccessControl
{
    public class ApplicationUser : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(70)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(254)]
        public string Email { get; set; }

        #region Navigation
        public int? DepartmentId { get; set; }
        public Department Department { get; set; } = null;
        public ICollection<RoleUserLink> RoleLinks { get; set; } = new List<RoleUserLink>();
        #endregion

    }
}
