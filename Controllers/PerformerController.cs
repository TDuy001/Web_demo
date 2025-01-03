using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo3.Data;
using Demo3.Data.Entities;
using Demo3.ViewModels;

namespace Demo3.Controllers
{
    public class PerformerController : Controller
    {
        private readonly CourseDbContext _context;

        public PerformerController(CourseDbContext context)
        {
            _context = context;
        }

        // GET: Performers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";

            if (searchString != null)
            {
                pageNumber = 1; // Đã sửa
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var performers = from p in _context.Performers
                             .Include(p => p.EventPerformers) // Eager Loading
                             .ThenInclude(ep => ep.Event) // Eager Loading
                             select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                performers = performers.Where(p => p.Name.Contains(searchString)
                    || p.EventPerformers.Any(ep => ep.Event.Title.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    performers = performers.OrderByDescending(p => p.Name);
                    break;
                case "Type":
                    performers = performers.OrderBy(p => p.Type);
                    break;
                case "type_desc":
                    performers = performers.OrderByDescending(p => p.Type);
                    break;
                default:
                    performers = performers.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Performer>.CreateAsync(performers.AsNoTracking(), pageNumber ?? 1, pageSize)); // Đã sửa
        }

        // GET: Performer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performer = await _context.Performers
                .Include(p => p.EventPerformers) // Eager Loading
                .ThenInclude(ep => ep.Event) // Eager Loading
                .FirstOrDefaultAsync(m => m.PerformerId == id);
            if (performer == null)
            {
                return NotFound();
            }

            return View(performer);
        }

        // GET: Performer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Performer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PerformerId,Name")] Performer performer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(performer);
        }

        // GET: Performer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performer = await _context.Performers
                .Include(p => p.EventPerformers) // Eager Loading
                .ThenInclude(ep => ep.Event) // Eager Loading
                .FirstOrDefaultAsync(p => p.PerformerId == id); // Đã sửa
            if (performer == null)
            {
                return NotFound();
            }
            return View(performer);
        }

        // POST: Performer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformerId,Name")] Performer performer)
        {
            if (id != performer.PerformerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformerExists(performer.PerformerId))
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
            return View(performer);
        }

        // GET: Performer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performer = await _context.Performers
                .Include(p => p.EventPerformers) // Eager Loading
                .ThenInclude(ep => ep.Event) // Eager Loading
                .FirstOrDefaultAsync(m => m.PerformerId == id);
            if (performer == null)
            {
                return NotFound();
            }

            return View(performer);
        }

        // POST: Performer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performer = await _context.Performers.FindAsync(id);
            if (performer != null)
            {
                _context.Performers.Remove(performer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformerExists(int id)
        {
            return _context.Performers.Any(e => e.PerformerId == id);
        }
    }
}
