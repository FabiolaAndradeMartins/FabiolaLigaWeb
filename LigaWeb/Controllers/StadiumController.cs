using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaWeb.Data.Entities;
using LigaWeb.Data.Repositories.Interfaces;

namespace LigaWeb.Controllers
{
    public class StadiumController : Controller
    {
        
        private readonly IStadiumRepository _stadiumRepo;

        public StadiumController(IStadiumRepository stadiumRepo)
        {            
            _stadiumRepo = stadiumRepo;
        }

        // GET: Stadium
        public async Task<IActionResult> Index()
        {
            var _result = _stadiumRepo.GetAll();
              return _result != null ? 
                          View(_result) :
                          Problem("Entity set 'DataContext.Stadiums'  is null.");
        }

        // GET: Stadium/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _stadiumRepo.GetByIdAsync(id.Value);
                
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // GET: Stadium/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stadium/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stadium stadium)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            if (ModelState.IsValid)
            {
                _stadiumRepo.CreateAsync(stadium);                
                return RedirectToAction(nameof(Index));
            }



            return View(stadium);
        }

        // GET: Stadium/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _stadiumRepo.GetByIdAsync(id.Value);
            if (stadium == null)
            {
                return NotFound();
            }
            return View(stadium);
        }

        // POST: Stadium/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Stadium stadium)
        {
            if (id != stadium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _stadiumRepo.UpdateAsync(stadium);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await StadiumExists(stadium.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stadium);
        }

        // GET: Stadium/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _stadiumRepo.GetByIdAsync(id.Value);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stadium = await _stadiumRepo.GetByIdAsync(id);
            if (stadium != null)
            {
                await _stadiumRepo.DeleteAsync(stadium);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StadiumExists(int id)
        {
          return await _stadiumRepo.ExistAsync(id);
        }
    }
}
