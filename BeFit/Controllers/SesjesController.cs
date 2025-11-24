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
            return View(await _context.Sesje
                .Where(s => s.UserId == userId)
                .ToListAsync());
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
    }
}
