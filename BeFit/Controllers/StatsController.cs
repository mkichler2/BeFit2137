using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats = await _context.Mix
                .Include(m => m.Typy)
                .Include(m => m.Sesje)
                .Where(m => m.Sesje.Start >= fourWeeksAgo)
                .GroupBy(m => m.Typy.Name)
                .Select(g => new StatsModel
                {
                    NazwaTypu = g.Key,
                    IloscWykonan = g.Count(),
                    LaczniePowtorzen = g.Sum(x => x.Serie * x.Powtorzenia),
                    SrednieObciazenie = (float)g.Average(x => x.Waga),
                    MaksymalneObciazenie = g.Max(x => x.Waga)
                })
                .ToListAsync();

            return View(stats);
        }
    }
}
