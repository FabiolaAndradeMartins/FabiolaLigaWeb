using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Models
{
    public class AddUserViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Club")]
        public int? ClubId { get; set; }

        public List<SelectListItem> Clubs { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public string? PhotoPath { get; set; }
    }

}
