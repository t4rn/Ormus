using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ormus.Web.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Login { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(64, MinimumLength = 5)]
        public string Email { get; set; }

        public UserRoleModel Role { get; set; }

        public int RoleId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Ghost { get; set; }

        [Display(Name = " User Role")]
        public IEnumerable<SelectListItem> UserRolesCollection { get; set; }
    }
}
