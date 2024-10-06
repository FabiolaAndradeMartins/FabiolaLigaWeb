using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
