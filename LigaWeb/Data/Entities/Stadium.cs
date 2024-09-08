using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Data.Entities
{
    public class Stadium : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Address { get; set; }
        public int Capacity { get; set; }

        public Club Club{ get; set; }
    }
}
