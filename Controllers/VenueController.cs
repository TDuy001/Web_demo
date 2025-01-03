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
    public class VenueController : Controller
    {
        private readonly CourseDbContext _context;

        public VenueController(CourseDbContext context)
        {
            _context = context;
        }

        // GET: Venues 
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LocationSortParm"] = sortOrder == "Location" ? "location_desc" : "Location";

            if (searchString != null)
            {
                pageNumber = 1; // Đã sửa
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var venues = from v in _context.Venues
                         .Include(v => v.Events) // Eager Loading
                         select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(v => v.Name.Contains(searchString) || v.Location.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    venues = venues.OrderByDescending(v => v.Name);
                    break;
                case "Location":
                    venues = venues.OrderBy(v => v.Location);
                    break;
                case "location_desc":
                    venues = venues.OrderByDescending(v => v.Location);
                    break;
                default:
                    venues = venues.OrderBy(v => v.Name);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Venue>.CreateAsync(venues.AsNoTracking(), pageNumber ?? 1, pageSize)); // Đã sửa
        }

        // GET: Venue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .Include(v => v.Events) // Eager Loading
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,Name,Location")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .Include(v => v.Events) // Eager Loading
                .FirstOrDefaultAsync(v => v.VenueId == id); // Đã sửa
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("VenueId,Name,Location,RowVersion")] Venue venue) // Đã sửa
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)

                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Venue)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Venue was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Venue)databaseEntry.ToObject();
                        ModelState.AddModelError("", "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this record, click "
                        + "the Save button again.");
                        venue.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }
        // GET: Venue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .Include(v => v.Events) // Eager Loading
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
