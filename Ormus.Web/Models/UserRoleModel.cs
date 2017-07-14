using System;
using System.ComponentModel.DataAnnotations;

namespace Ormus.Web.Models
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Role Code")]
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public bool Ghost { get; set; }
    }
}
