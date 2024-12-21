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
        [Range(1900, 2006, ErrorMessage = "Year of birth must be between 1900 and 2006 (at least 18 years old).")]
        public int YearOfBirth { get; set; }

        [Required]
        [Range(1.50, 3.00, ErrorMessage = "Height must be at least 1.50 meters.")]
        public decimal Height { get; set; }

        [StringLength(400)]
        public string Photo { get; set; }

        
        public int? ClubId { get; set; }
        public Club? Club { get; set; }
    }
}
