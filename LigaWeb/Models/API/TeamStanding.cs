namespace LigaWeb.Models.API
{
    public class TeamStanding
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int Points { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
    }
}
