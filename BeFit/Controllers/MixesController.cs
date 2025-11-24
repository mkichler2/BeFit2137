using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using BeFit.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class MixesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MixesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private void LoadLists()
        {
            var userId = GetUserId();

            ViewBag.SesjeId = _context.Sesje
                .Where(s => s.UserId == userId)
                .AsEnumerable()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Start.ToString("g")
                })
                .ToList();

            ViewBag.TypyId = _context.Typy
                .AsEnumerable()
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToList();
        }

        // GET: Mixes
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var items = await _context.Mix
                .Include(m => m.Sesje)
                .Include(m => m.Typy)
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return View(items);
        }

        // GET: Mixes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mix = await _context.Mix
                .Include(m => m.Sesje)
                .Include(m => m.Typy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mix == null || mix.UserId != GetUserId()) return NotFound();

            return View(mix);
        }

        // GET: Mixes/Create
        public IActionResult Create()
        {
            LoadLists();
            return View();
        }

        // POST: Mixes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMixDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadLists();
                return View(dto);
            }

            // Sprawdzenie czy sesja należy do użytkownika
            var session = await _context.Sesje.FirstOrDefaultAsync(s => s.Id == dto.SesjeId);
            if (session == null || session.UserId != GetUserId()) return NotFound();

            var mix = new Mix
            {
                TypyId = dto.TypyId,
                SesjeId = dto.SesjeId,
                Waga = dto.Waga,
                Serie = dto.Serie,
                Powtorzenia = dto.Powtorzenia,
                UserId = GetUserId()
            };

            _context.Add(mix);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Mixes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mix = await _context.Mix.FindAsync(id);
            if (mix == null || mix.UserId != GetUserId()) return NotFound();

            LoadLists();
            return View(mix);
        }

        // POST: Mixes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mix mix)
        {
            if (id != mix.Id) return NotFound();

            var existing = await _context.Mix.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (existing == null || existing.UserId != GetUserId()) return NotFound();

            mix.UserId = existing.UserId; // nie pozwalamy manipulować UserId

            if (!ModelState.IsValid)
            {
                LoadLists();
                return View(mix);
            }

            _context.Update(mix);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Mixes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mix = await _context.Mix
                .Include(m => m.Sesje)
                .Include(m => m.Typy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mix == null || mix.UserId != GetUserId()) return NotFound();

            return View(mix);
        }

        // POST: Mixes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mix = await _context.Mix.FindAsync(id);
            if (mix == null || mix.UserId != GetUserId()) return NotFound();

            _context.Mix.Remove(mix);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
