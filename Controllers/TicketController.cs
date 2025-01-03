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
    public class TicketController : Controller
    {
        private readonly CourseDbContext _context;

        public TicketController(CourseDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewData["EventSortParm"] = sortOrder == "Event" ? "event_desc" : "Event";

            if (searchString != null)
            {
                pageNumber = 1; // Đã sửa
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var tickets = from t in _context.Tickets
                            .Include(t => t.Event) // Eager Loading
                            .Include(t => t.Order) // Eager Loading
                          select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(t => t.Event.Title.Contains(searchString)
                    || t.Order.Customer.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "price_desc":
                    tickets = tickets.OrderByDescending(t => t.Price);
                    break;
                case "Event":
                    tickets = tickets.OrderBy(t => t.Event.Title);
                    break;
                case "event_desc":
                    tickets = tickets.OrderByDescending(t => t.Event.Title);
                    break;
                default:
                    tickets = tickets.OrderBy(t => t.Price);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Ticket>.CreateAsync(tickets.AsNoTracking(), pageNumber ?? 1, pageSize)); // Đã sửa
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Event) // Eager Loading
                .Include(t => t.Order) // Eager Loading
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Ticket/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            return View();
        }

        // POST: Ticket/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,EventId,OrderId,Price")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", ticket.EventId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", ticket.OrderId);
            return View(ticket);
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Event) // Eager Loading
                .Include(t => t.Order) // Eager Loading
                .FirstOrDefaultAsync(t => t.TicketId == id); // Đã sửa
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", ticket.EventId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", ticket.OrderId);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,EventId,OrderId,Price,RowVersion")] Ticket ticket) // Đã sửa 
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Ticket)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Ticket was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Ticket)databaseEntry.ToObject();
                        ModelState.AddModelError("", "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this record, click "
                        + "the Save button again.");
                        ticket.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", ticket.EventId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", ticket.OrderId);
            return View(ticket);
        }

        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Event) // Eager Loading
                .Include(t => t.Order) // Eager Loading
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
