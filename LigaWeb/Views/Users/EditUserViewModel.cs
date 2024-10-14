using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Role { get; set; } // Role atual do usuário
    }
}
