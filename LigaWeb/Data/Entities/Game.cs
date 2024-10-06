using System.ComponentModel.DataAnnotations;

namespace LigaWeb.Data.Entities
{
    public class Game : IEntity
    {
        [Key]
        public int Id { get; set; }

		[Required(ErrorMessage = "A data e hora são obrigatórias")]
		[DataType(DataType.DateTime, ErrorMessage = "Insira uma data e hora válidas")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int StadiumId { get; set; }
        public Stadium? Stadium { get; set; }

        [Required]
        public int HostClubId { get; set; }
        public Club? HostClub { get; set; }

        [Required]
        public int VisitingClubId { get; set; }
        public Club? VisitingClub { get; set; }

        /* Game Statistics */

        public int HostsFouls { get; set; } = 0;
        public int VistorsFouls { get; set; } = 0;

        public int HostsPenalties { get; set; } = 0;
        public int VistorsPenalties { get; set; } = 0;

        public int HostsYellowCards { get; set; } = 0;
        public int VistorsYellowCards { get; set; } = 0;

        public int HostsRedCards { get; set; } = 0;
        public int VistorsRedCards { get; set; } = 0;

        public int HostsGoalKicks { get; set; } = 0;
        public int VistorsGoalKicks { get; set; } = 0;

        public int HostsGoals { get; set; } = 0;
        public int VistorsGoals { get; set; } = 0;

        public Game()
        {
            HostsFouls = VistorsFouls = HostsPenalties = VistorsPenalties = HostsYellowCards =
                VistorsYellowCards = HostsRedCards = VistorsRedCards = HostsGoalKicks =
                VistorsGoalKicks = HostsGoals = VistorsGoals = 0;
        }
    }
}
