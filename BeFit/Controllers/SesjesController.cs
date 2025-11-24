using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using BeFit.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class SesjesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesjesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: Sesjes
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var items = await _context.Sesje
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return View(items);
        }

        // GET: Sesjes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Sesje.FirstOrDefaultAsync(s => s.Id == id);
            if (item == null || item.UserId != GetUserId()) return NotFound();

            return View(item);
        }

        // GET: Sesjes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sesjes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSesjaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var sesja = new Sesje
            {
                Start = dto.Start,
                End = dto.End,
                UserId = GetUserId()
            };

            _context.Add(sesja);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Sesjes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Sesje.FindAsync(id);
            if (item == null || item.UserId != GetUserId()) return NotFound();

            return View(item);
        }

        // POST: Sesjes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sesje sesje)
        {
            if (id != sesje.Id) return NotFound();

            var existing = await _context.Sesje.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (existing == null || existing.UserId != GetUserId()) return NotFound();

            sesje.UserId = existing.UserId;

            if (!ModelState.IsValid)
                return View(sesje);

            _context.Update(sesje);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Sesjes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Sesje.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null || item.UserId != GetUserId()) return NotFound();

            return View(item);
        }

        // POST: Sesjes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Sesje.FindAsync(id);
            if (item == null || item.UserId != GetUserId()) return NotFound();

            _context.Sesje.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
