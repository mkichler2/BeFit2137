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

        // GET: Mixes/Create
        public IActionResult Create()
        {
            var userId = GetUserId();

            // Sesje tylko dla aktualnego usera, zamieniamy DateTime na czytelny string
            var sesjeList = _context.Sesje
                .Where(s => s.UserId == userId)
                .AsEnumerable()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Start.ToString("g") // format: krótka data + czas
                })
                .ToList();

            var typyList = _context.Typy
                .AsEnumerable()
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                })
                .ToList();

            ViewBag.SesjeId = sesjeList;
            ViewBag.TypyId = typyList;

            return View();
        }

        // POST: Mixes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMixDto dto)
        {
            if (!ModelState.IsValid)
            {
                // jeśli walidacja nie przeszła -> trzeba ponownie wypełnić listy,
                // inaczej selecty będą puste
                var userId = GetUserId();

                var sesjeList = _context.Sesje
                    .Where(s => s.UserId == userId)
                    .AsEnumerable()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Start.ToString("g")
                    })
                    .ToList();

                var typyList = _context.Typy
                    .AsEnumerable()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    })
                    .ToList();

                ViewBag.SesjeId = sesjeList;
                ViewBag.TypyId = typyList;

                return View(dto);
            }

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
    }
}
