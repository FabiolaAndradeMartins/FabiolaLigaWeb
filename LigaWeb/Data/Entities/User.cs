using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // FK para o clube (opcional)
        public int? ClubId { get; set; }
        public Club? Club { get; set; }

        // Caminho da foto do usuário
        [StringLength(400)]
        public string? PhotoPath { get; set; }
    }
}
