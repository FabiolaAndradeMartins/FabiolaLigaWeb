using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaWeb.Data.Entities;
using LigaWeb.Helpers.Impl;
using LigaWeb.Models;
using LigaWeb.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using LigaWeb.Data;

namespace LigaWeb.Controllers
{
    public class PlayersController : Controller
    {        
        private readonly IPlayerRepository _playerRepository;
        private readonly IClubRepository _clubRepository;
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public PlayersController(IPlayerRepository layerRepository, IClubRepository clubRepository, DataContext context, IUserService userService)
        {
            _playerRepository = layerRepository;
            _clubRepository = clubRepository;
            _context = context;
            _userService = userService;
        }

        // GET: Players
        //[Authorize(Roles = "Club")]
        //public async Task<IActionResult> Index()
        //{            
        //    var result = _playerRepository.GetAll().ToList();
        //    return View(result);
        //}

        // GET: Players/Details/5
        //[Authorize(Roles = "Club")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" });
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player.ClubId != null)
            {
                player.Club = await _clubRepository.GetByIdAsync(player.ClubId.Value);
            }

            if (player == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" });
            }

            return View(player);
        }

        // GET: Players/Index?clubId=5
        public async Task<IActionResult> Index(int? clubId)
        {
            List<Player> players = new List<Player>();           

            if (clubId != null)
                players = await _context.Players
                    .Include(p => p.Club)
                    .Where(x => x.ClubId == clubId)
                    .ToListAsync();
            else
                players = await _context.Players
                    .Include(p => p.Club)
                    .ToListAsync();


            return View(players);
        }

        // GET: Players/Create
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();

            if (currentUser != null && currentUser.ClubId.HasValue)
            {
                var clubs = _clubRepository
                    .GetAll()
                    .Where(o => o.Id == currentUser.ClubId);

                ViewData["ClubId"] = new SelectList(clubs, "Id", "Name");
            }
            else
            {
                ViewData["ClubId"] = new SelectList(_clubRepository.GetAll(), "Id", "Name");
            }
            
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Create([Bind("Id,Name,YearOfBirth,Height,Photo,ClubId")] Player player)
        {
            if (ModelState.IsValid)
            {                
                await _playerRepository.CreateAsync(player);

                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_clubRepository.GetAll(), "Id", "Name", player.ClubId);
            return View(player);
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" });
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" }); ;
            }
            ViewData["ClubId"] = new SelectList(_clubRepository.GetAll(), "Id", "Name", player.ClubId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,YearOfBirth,Height,Photo,ClubId")] Player player)
        {
            if (id != player.Id)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _playerRepository.UpdateAsync(player);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await PlayerExists(player.Id)))
                    {
                        return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                        new ErrorNotFoundViewModel { Id = id, Message = "Player not found" });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_clubRepository.GetAll(), "Id", "Name", player.ClubId);
            return View(player);
        }

        // GET: Players/Delete/5
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" }); ;
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Player not found" }); ;
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var player = await _playerRepository.GetByIdAsync(id);

            if (player != null)
            {
                await _playerRepository.DeleteAsync(player);
            }            
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PlayerExists(int id)
        {
            return await _playerRepository.ExistAsync(id);
        }
    }
}
