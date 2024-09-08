using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Data.Entities
{
    public class Player : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        [Required]
        public decimal Height { get; set; }

        [StringLength(400)]
        public string Photo { get; set; }

        [Required]
        public Club Club { get; set; }
    }
}
