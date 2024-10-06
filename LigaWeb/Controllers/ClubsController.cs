using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaWeb.Data;
using LigaWeb.Data.Entities;
using LigaWeb.Helpers.Impl;
using LigaWeb.Models;
using LigaWeb.Data.Repositories.Interfaces;

namespace LigaWeb.Controllers
{
    public class ClubsController : Controller
    {
        private readonly DataContext _context;
        private readonly IClubRepository _clubRepository;

        public ClubsController(DataContext context, IClubRepository clubRepository)
        {
            _context = context;
            _clubRepository = clubRepository;
        }

        // GET: Clubs
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Clubs.Include(c => c.Stadium);
            return View(await dataContext.ToListAsync());
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }

            var club = await _context.Clubs
                .Include(c => c.Stadium)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }

            return View(club);
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            ViewData["StadiumId"] = new SelectList(_context.Stadiums, "Id", "Name");
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StadiumId"] = new SelectList(_context.Stadiums, "Id", "Name", club.StadiumId);
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }

            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }
            ViewData["StadiumId"] = new SelectList(_context.Stadiums, "Id", "Name", club.StadiumId);
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Nickname,YearOfFoundation,Location,Coach,TeamEmblem,StadiumId")] Club club)
        {
            if (id != club.Id)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.Id))
                    {
                        return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StadiumId"] = new SelectList(_context.Stadiums, "Id", "Name", club.StadiumId);
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }

            var club = await _context.Clubs
                .Include(c => c.Stadium)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return new NotFoundViewResult("~/Views/Error/ErrorNotFound.cshtml",
                    new ErrorNotFoundViewModel { Id = id, Message = "Club not found" });
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clubs == null)
            {
                return Problem("Entity set 'DataContext.Clubs'  is null.");
            }
            var club = await _context.Clubs.FindAsync(id);
            if (club != null)
            {
                _context.Clubs.Remove(club);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
          return (_context.Clubs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
