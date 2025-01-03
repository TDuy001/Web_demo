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
    public class EventController : Controller
    {
        private readonly CourseDbContext _context;

        public EventController(CourseDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            // Kiểm tra nếu có từ khóa tìm kiếm 
            if (searchString != null)
            {
                // Đặt trang hiện tại về 1 khi có tìm kiếm mới 
                pageNumber = 1; // Đã sửa
            }
            else
            {
                // Sử dụng từ khóa tìm kiếm hiện tại nếu không có tìm kiếm mới 
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var events = from e in _context.Events
                            .Include(e => e.Category) // Eager Loading
                            .Include(e => e.Venue) // Eager Loading
                         select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.Title.Contains(searchString)
                    || e.Category.Name.Contains(searchString)
                    || e.Venue.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    events = events.OrderByDescending(e => e.Title);
                    break;
                case "Date":
                    events = events.OrderBy(e => e.Date);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(e => e.Date);
                    break;
                default:
                    events = events.OrderBy(e => e.Title);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Event>.CreateAsync(events.AsNoTracking(), pageNumber ?? 1, pageSize)); // Đã sửa
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category) // Eager Loading
                .Include(e => e.Venue) // Eager Loading
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Location");
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Title,Date,VenueId,CategoryId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Location", @event.VenueId);
            return View(@event);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category) // Eager Loading
                .Include(e => e.Venue) // Eager Loading
                .FirstOrDefaultAsync(e => e.EventId == id); // Đã sửa
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Location", @event.VenueId);
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,Date,VenueId,CategoryId,RowVersion")] Event @event) // Đã sửa
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Event)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Event was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Event)databaseEntry.ToObject();
                        ModelState.AddModelError("",
                         "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this record, click "
                        + "the Save button again.");
                        @event.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Location", @event.VenueId);
            return View(@event);
        }


        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category) // Eager Loading
                .Include(e => e.Venue) // Eager Loading
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
