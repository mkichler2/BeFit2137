using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats = await _context.Mix
                .Include(m => m.Typy)
                .Include(m => m.Sesje)
                //FILTROWANIE PO UZYTKOWNIKU NIZEJ JEST
                .Where(m => m.UserId == userId)                
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
