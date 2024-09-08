using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Data.Entities
{
    public class Game : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Stadium Stadium { get; set; }

        [Required]
        public int HostClubId { get; set; }
        public Club HostClub { get; set; }

        [Required]
        public int VisitingClubId { get; set; }
        public Club VisitingClub { get; set; }

        /* Game Statistics */

        public int HostsFouls { get; set; }
        public int VistorsFouls { get; set; }

        public int HostsPenalties { get; set; }
        public int VistorsPenalties { get; set; }

        public int HostsYellowCards { get; set; }
        public int VistorsYellowCards { get; set; }

        public int HostsRedCards { get; set; }
        public int VistorsRedCards { get; set; }

        public int HostsGoalKicks { get; set; }
        public int VistorsGoalKicks { get; set; }

        public int HostsGoals { get; set; }
        public int VistorsGoals { get; set; }
    }
}
