using LigaWeb.Data;
using LigaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LigaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Obter todos os jogos
            var games = _context.Games
                .Include(g => g.HostClub)
                .Include(g => g.VisitingClub)
                .ToList();

            // Calcular pontuação

            var query = _context.Clubs.ToList();

            var clubStats = query
                .Select(club => new
                {
                    Club = club,
                    Points = games.Where(g => g.HostClubId == club.Id && g.HostsGoals > g.VistorsGoals).Count() * 3 +
                             games.Where(g => g.VisitingClubId == club.Id && g.VistorsGoals > g.HostsGoals).Count() * 3 +
                             games.Where(g => g.HostClubId == club.Id && g.HostsGoals == g.VistorsGoals).Count() +
                             games.Where(g => g.VisitingClubId == club.Id && g.HostsGoals == g.VistorsGoals).Count(),

                    GoalsScored = games.Where(g => g.HostClubId == club.Id).Sum(g => g.HostsGoals) +
                                  games.Where(g => g.VisitingClubId == club.Id).Sum(g => g.VistorsGoals),

                    GoalsConceded = games.Where(g => g.HostClubId == club.Id).Sum(g => g.VistorsGoals) +
                                    games.Where(g => g.VisitingClubId == club.Id).Sum(g => g.HostsGoals),

                    YellowCards = games.Where(g => g.HostClubId == club.Id).Sum(g => g.HostsYellowCards) +
                                  games.Where(g => g.VisitingClubId == club.Id).Sum(g => g.VistorsYellowCards),

                    RedCards = games.Where(g => g.HostClubId == club.Id).Sum(g => g.HostsRedCards) +
                               games.Where(g => g.VisitingClubId == club.Id).Sum(g => g.VistorsRedCards),

                    GamesPlayed = games.Where(g => g.HostClubId == club.Id || g.VisitingClubId == club.Id).Count(),
                    GamesToPlay = games.Where(g => g.Date > DateTime.Now && (g.HostClubId == club.Id || g.VisitingClubId == club.Id)).Count()
                }).ToList();

            return View(clubStats.OrderByDescending(x => x.Points));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error(string message)
        {
            ViewData["Message"] = message;
            return View();
        }

        [HttpGet]

		public IActionResult AccessDenied(string returnUrl)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}
	}
}
