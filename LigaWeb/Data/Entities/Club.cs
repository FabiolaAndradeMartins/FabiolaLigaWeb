using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Data.Entities
{
    public class Club : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Nickname { get; set; }

        public int YearOfFoundation { get; set; }

        [StringLength(200,ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Location { get; set; }

        [StringLength(50,ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Coach { get; set; }

        [StringLength(400)]
        public string TeamEmblem { get; set; }

        public int StadiumId { get; set; }
        public Stadium Stadium { get; set; }

        public ICollection<Player> Players { get; set; }
        public ICollection<Game> HostGames { get; set; }
        public ICollection<Game> VisitingGames { get; set; }


    }
}
