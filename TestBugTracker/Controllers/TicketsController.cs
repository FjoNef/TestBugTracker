﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestBugTracker.Data;
using TestBugTracker.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace TestBugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly TrackerContext _context;

        public TicketsController(TrackerContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string asc, string sortColumn)
        {
            bool ascending = true;
            string column = "date";
            if (!String.IsNullOrEmpty(asc))
            {                
                ascending = Convert.ToBoolean(asc);
            }
            if (!String.IsNullOrEmpty(sortColumn))
            {
                column = sortColumn;
            }
            var tickets = _context.Tickets.Include(t => t.User).OrderByDescending(t=>t.DateCreation);

            switch (column)
            {
                case "short":
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.ShortDescription);
                    else
                        tickets = tickets.OrderByDescending(t => t.ShortDescription);                    
                    break;
                case "detailed":
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.DetailedDescription);
                    else
                        tickets = tickets.OrderByDescending(t => t.DetailedDescription);
                    break;
                case "user":
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.User.FullName);
                    else
                        tickets = tickets.OrderByDescending(t => t.User.FullName);
                    break;
                case "status":
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.Status);
                    else
                        tickets = tickets.OrderByDescending(t => t.Status);
                    break;
                case "urgency":
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.Urgency);
                    else
                        tickets = tickets.OrderByDescending(t => t.Urgency);
                    break;
                case "criticality":
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.Criticality);
                    else
                        tickets = tickets.OrderByDescending(t => t.Criticality);
                    break;
                default:
                    if (ascending)
                        tickets = tickets.OrderBy(t => t.DateCreation);
                    break;
            }
            ViewData["SortOrder"] = (!ascending).ToString();
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.TicketHistories)
                    .ThenInclude(h => h.User)
                .SingleOrDefaultAsync(m => m.ID == ID);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,DateCreation,ShortDescription,DetailedDescription,Status,Urgency,Criticality")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
                ticket.UserID = user.ID;
                ticket.DateCreation = DateTime.Now;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                await CreateHistory(ticket, "Created new ticket");
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.TicketHistories)
                    .ThenInclude(h=>h.User)
                .SingleOrDefaultAsync(m => m.ID == ID);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ID, [Bind("ID,UserID,DateCreation,ShortDescription,DetailedDescription,Status,Urgency,Criticality")] Ticket ticket)
        {
            if (ID != ticket.ID)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.ID))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.User)
                .SingleOrDefaultAsync(m => m.ID == ID);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var ticket = await _context.Tickets.SingleOrDefaultAsync(m => m.ID == ID);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeStatus(int? ID)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int ID, string comment, string down)
        {
            var ticket = await _context.Tickets.SingleOrDefaultAsync(m => m.ID == ID);
            if (ticket == null)
            {
                return NotFound();
            }

            if (Boolean.Parse(down))
            {
                ticket.DownStatus();
            }
            else
            {
                ticket.UpStatus();
            }

            _context.Update(ticket);
            await _context.SaveChangesAsync();
            await CreateHistory(ticket, comment);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int ID)
        {
            return _context.Tickets.Any(e => e.ID == ID);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task CreateHistory(Ticket ticket, string comment)
        {
            var history = new TicketHistory();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);

            history.DateOfChange = DateTime.Now;
            history.Action = (Models.Action)(int)ticket.Status;
            history.UserID = user.ID;
            history.TicketID = ticket.ID;
            history.Comment = comment;

            _context.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
