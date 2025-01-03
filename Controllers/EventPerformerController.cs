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
    public class EventPerformerController : Controller
    {
        private readonly CourseDbContext _context;

        public EventPerformerController(CourseDbContext context)
        {
            _context = context;
        }

        // GET: EventPerformers 
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["EventSortParm"] = String.IsNullOrEmpty(sortOrder) ? "event_desc" : "";
            ViewData["PerformerSortParm"] = sortOrder == "Performer" ? "performer_desc" : "Performer";

            if (searchString != null)
            {
                pageNumber = 1; // Đã sửa
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var eventPerformers = from ep in _context.EventPerformers
                                    .Include(ep => ep.Event) // Eager Loading
                                    .Include(ep => ep.Performer) // Eager Loading
                                  select ep;

            if (!String.IsNullOrEmpty(searchString))
            {
                eventPerformers = eventPerformers.Where(ep => ep.Event.Title.Contains(searchString)
                    || ep.Performer.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "event_desc":
                    eventPerformers = eventPerformers.OrderByDescending(ep => ep.Event.Title);
                    break;
                case "Performer":
                    eventPerformers = eventPerformers.OrderBy(ep => ep.Performer.Name);
                    break;
                case "performer_desc":
                    eventPerformers = eventPerformers.OrderByDescending(ep => ep.Performer.Name);
                    break;
                default:
                    eventPerformers = eventPerformers.OrderBy(ep => ep.Event.Title);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<EventPerformer>.CreateAsync(eventPerformers.AsNoTracking(), pageNumber ?? 1, pageSize)); // Đã sửa
        }

        // GET: EventPerformer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPerformer = await _context.EventPerformers
                .Include(ep => ep.Event) // Eager Loading
                .Include(ep => ep.Performer) // Eager Loading
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventPerformer == null)
            {
                return NotFound();
            }

            return View(eventPerformer);
        }

        // GET: EventPerformer/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title");
            ViewData["PerformerId"] = new SelectList(_context.Performers, "PerformerId", "Name");
            return View();
        }

        // POST: EventPerformer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,PerformerId")] EventPerformer eventPerformer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventPerformer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", eventPerformer.EventId);
            ViewData["PerformerId"] = new SelectList(_context.Performers, "PerformerId", "Name", eventPerformer.PerformerId);
            return View(eventPerformer);
        }

        // GET: EventPerformer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPerformer = await _context.EventPerformers
                .Include(ep => ep.Event) // Eager Loading
                .Include(ep => ep.Performer) // Eager Loading
                .FirstOrDefaultAsync(ep => ep.EventId == id); // Đã sửa
            if (eventPerformer == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", eventPerformer.EventId);
            ViewData["PerformerId"] = new SelectList(_context.Performers, "PerformerId", "Name", eventPerformer.PerformerId);
            return View(eventPerformer);
        }

        // POST: EventPerformer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,PerformerId")] EventPerformer eventPerformer)
        {
            if (id != eventPerformer.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventPerformer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventPerformerExists(eventPerformer.EventId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", eventPerformer.EventId);
            ViewData["PerformerId"] = new SelectList(_context.Performers, "PerformerId", "Name", eventPerformer.PerformerId);
            return View(eventPerformer);
        }

        // GET: EventPerformer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPerformer = await _context.EventPerformers
                .Include(ep => ep.Event) // Eager Loading
                .Include(ep => ep.Performer) // Eager Loading
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventPerformer == null)
            {
                return NotFound();
            }

            return View(eventPerformer);
        }

        // POST: EventPerformer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventPerformer = await _context.EventPerformers.FindAsync(id);
            if (eventPerformer != null)
            {
                _context.EventPerformers.Remove(eventPerformer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventPerformerExists(int id)
        {
            return _context.EventPerformers.Any(e => e.EventId == id);
        }
    }
}
