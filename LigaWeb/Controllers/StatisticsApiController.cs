using LigaWeb.Data;
using LigaWeb.Models.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LigaWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsApiController : ControllerBase
    {
        private readonly DataContext _context;

        public StatisticsApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamStanding>>> GetStandings()
        {
            var games = await _context.Games
                .Include(g => g.HostClub)
                .Include(g => g.VisitingClub)
                .ToListAsync();

            var standings = new Dictionary<int, TeamStanding>();

            foreach (var game in games)
            {
                // Processa o time anfitrião (Host)
                if (!standings.ContainsKey(game.HostClubId))
                {
                    standings[game.HostClubId] = new TeamStanding
                    {
                        ClubId = game.HostClubId,
                        ClubName = game.HostClub!.Name
                    };
                }

                // Processa o time visitante (Visiting)
                if (!standings.ContainsKey(game.VisitingClubId))
                {
                    standings[game.VisitingClubId] = new TeamStanding
                    {
                        ClubId = game.VisitingClubId,
                        ClubName = game.VisitingClub!.Name
                    };
                }

                var hostStanding = standings[game.HostClubId];
                var visitingStanding = standings[game.VisitingClubId];

                // Atualiza estatísticas de jogos e golos
                hostStanding.GamesPlayed++;
                hostStanding.GoalsFor += game.HostsGoals;
                hostStanding.GoalsAgainst += game.VistorsGoals;

                visitingStanding.GamesPlayed++;
                visitingStanding.GoalsFor += game.VistorsGoals;
                visitingStanding.GoalsAgainst += game.HostsGoals;

                // Determina o vencedor e distribui os pontos
                if (game.HostsGoals > game.VistorsGoals)
                {
                    hostStanding.Wins++;
                    hostStanding.Points += 3;
                    visitingStanding.Losses++;
                }
                else if (game.HostsGoals < game.VistorsGoals)
                {
                    visitingStanding.Wins++;
                    visitingStanding.Points += 3;
                    hostStanding.Losses++;
                }
                else
                {
                    // Empate
                    hostStanding.Draws++;
                    hostStanding.Points++;
                    visitingStanding.Draws++;
                    visitingStanding.Points++;
                }
            }

            // Retorna a lista de classificação ordenada por pontos (desc) e saldo de golos
            var standingsList = standings.Values
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenByDescending(s => s.GoalsFor)
                .ToList();

            return Ok(standingsList);
        }
    }
}
