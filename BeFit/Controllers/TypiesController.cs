using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;

namespace BeFit.Controllers
{
    public class TypiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Typies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Typy.ToListAsync());
        }

        // GET: Typies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typy = await _context.Typy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typy == null)
            {
                return NotFound();
            }

            return View(typy);
        }

        // GET: Typies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Typies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Typy typy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typy);
        }

        // GET: Typies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typy = await _context.Typy.FindAsync(id);
            if (typy == null)
            {
                return NotFound();
            }
            return View(typy);
        }

        // POST: Typies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Typy typy)
        {
            if (id != typy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypyExists(typy.Id))
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
            return View(typy);
        }

        // GET: Typies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typy = await _context.Typy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typy == null)
            {
                return NotFound();
            }

            return View(typy);
        }

        // POST: Typies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typy = await _context.Typy.FindAsync(id);
            if (typy != null)
            {
                _context.Typy.Remove(typy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypyExists(int id)
        {
            return _context.Typy.Any(e => e.Id == id);
        }
    }
}
