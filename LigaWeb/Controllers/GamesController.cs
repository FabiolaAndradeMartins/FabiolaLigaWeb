using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaWeb.Data;
using LigaWeb.Data.Entities;
using LigaWeb.Models;
using Newtonsoft.Json;
using LigaWeb.Data.Repositories.Interfaces;
using LigaWeb.Helpers.Impl;
using Microsoft.AspNetCore.Authorization;

namespace LigaWeb.Controllers
{
    
    public class GamesController : Controller
    {        
        private readonly IClubRepository _clubRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IStadiumRepository _stadiumRepository;

        public GamesController(IClubRepository clubRepository, IGameRepository gameRepository, IStadiumRepository stadiumRepository)
        {
            _clubRepository = clubRepository;
            _gameRepository = gameRepository;
            _stadiumRepository = stadiumRepository;
        }

        // GET: Games
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            var games = _gameRepository
                .GetAll(x => x.HostClub, x => x.Stadium, x => x.VisitingClub);

			// Preparar os dados para o Gráfico de Barras
			var barChartData = new List<object>
		    {
			    new object[] { "Statistic", "Host", "Visitor" },
			    new object[] { "Faults", games.Sum(g => g.HostsFouls), games.Sum(g => g.VistorsFouls) },
			    new object[] { "Penalties", games.Sum(g => g.HostsPenalties), games.Sum(g => g.VistorsPenalties) },
			    new object[] { "Yellow Cards", games.Sum(g => g.HostsYellowCards), games.Sum(g => g.VistorsYellowCards) },
			    new object[] { "Red Cards", games.Sum(g => g.HostsRedCards), games.Sum(g => g.VistorsRedCards) },
			    new object[] { "Goal Kicks", games.Sum(g => g.HostsGoalKicks), games.Sum(g => g.VistorsGoalKicks) },
			    new object[] { "Goals", games.Sum(g => g.HostsGoals), games.Sum(g => g.VistorsGoals) }
		    };


            // Agregar o número de jogos por data
            var jogosPorData = games
                .GroupBy(g => g.Date.Date)
                .Select(g => new { Data = g.Key, Quantidade = g.Count() })
                .OrderBy(g => g.Data);            

            // Serializar os dados para JSON
            var viewModel = new GamesIndexViewModel
			{
				Games = games,
				BarChartData = JsonConvert.SerializeObject(barChartData)
            };

			return View(viewModel);            
        }

        // GET: Games/Details/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }

            var game = await _gameRepository
                .GetByIdAsync(id.Value, x => x.HostClub, x => x.Stadium, x => x.VisitingClub);

            if (game == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }

            // Preparar os dados para o Google Charts
            var chartData = new List<object>
            {
                new object[] { "Statistics", game.HostClub?.Name ?? "Host Club", game.VisitingClub?.Name ?? "Visitor Club" },
                new object[] { "Fouls", game.HostsFouls, game.VistorsFouls },
                new object[] { "Penalties", game.HostsPenalties, game.VistorsPenalties },
                new object[] { "Yellow Cards", game.HostsYellowCards, game.VistorsYellowCards },
                new object[] { "Red Cards", game.HostsRedCards, game.VistorsRedCards },
                new object[] { "Goal Kicks", game.HostsGoalKicks, game.VistorsGoalKicks },
                new object[] { "Goals", game.HostsGoals, game.VistorsGoals }
            };

            var viewModel = new GameDetailsViewModel
            {
                Game = game,
                ChartData = JsonConvert.SerializeObject(chartData)
            };

            return View(viewModel);
        }

        // GET: Games/Create
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            CreateViewBagDropDown();

            return View(new Game());
        }

        private void CreateViewBagDropDown()
        {
            var clubs = _clubRepository.GetAll();
            var stadiums = _stadiumRepository.GetAll();

            ViewData["HostClubId"] = new SelectList(clubs, "Id", "Name");
            ViewData["StadiumId"] = new SelectList(stadiums, "Id", "Name");
            ViewData["VisitingClubId"] = new SelectList(clubs, "Id", "Name");
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([Bind("Id,Date,StadiumId,HostClubId,VisitingClubId,HostsFouls,VistorsFouls,HostsPenalties,VistorsPenalties,HostsYellowCards,VistorsYellowCards,HostsRedCards,VistorsRedCards,HostsGoalKicks,VistorsGoalKicks,HostsGoals,VistorsGoals")] Game game)
        {
            if (ModelState.IsValid)
            {
                await _gameRepository.CreateAsync(game);
                return RedirectToAction(nameof(Index));
            }

            CreateViewBagDropDown();

            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }
            
            CreateViewBagDropDown();

            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,StadiumId,HostClubId,VisitingClubId,HostsFouls,VistorsFouls,HostsPenalties,VistorsPenalties,HostsYellowCards,VistorsYellowCards,HostsRedCards,VistorsRedCards,HostsGoalKicks,VistorsGoalKicks,HostsGoals,VistorsGoals")] Game game)
        {
            if (id != game.Id)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gameRepository.UpdateAsync(game);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await GameExists(game.Id)))
                    {
                        return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                            new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            CreateViewBagDropDown();

            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }            

            var game = await _gameRepository
                .GetByIdAsync(id.Value, x => x.HostClub, x => x.Stadium, x => x.VisitingClub);

            if (game == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Game not found" });
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var game = await _gameRepository
                .GetByIdAsync(id);

            if (game != null)
            {
                await _gameRepository.DeleteAsync(game);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GameExists(int id)
        {
            return await _gameRepository.ExistAsync(id);
        }
    }
}
