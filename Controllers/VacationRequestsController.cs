using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShiftON.Data;

namespace ShiftON.Controllers
{
    public class VacationRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacationRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VacationRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vacations.Include(v => v.Schedules);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VacationRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vacations == null)
            {
                return NotFound();
            }

            var vacationRequest = await _context.Vacations
                .Include(v => v.Schedules)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationRequest == null)
            {
                return NotFound();
            }

            return View(vacationRequest);
        }

        // GET: VacationRequests/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId");
            return View();
        }

        // POST: VacationRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestDay,ScheduleId")] VacationRequest vacationRequest)
        {
            if (ModelState.IsValid)
            {
                vacationRequest.RequestDay = DateTime.Now;
                _context.Add(vacationRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId", vacationRequest.ScheduleId);
            return View(vacationRequest);
        }

        // GET: VacationRequests/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vacations == null)
            {
                return NotFound();
            }

            var vacationRequest = await _context.Vacations.FindAsync(id);
            if (vacationRequest == null)
            {
                return NotFound();
            }
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId", vacationRequest.ScheduleId);
            return View(vacationRequest);
        }

        // POST: VacationRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestDay,ScheduleId")] VacationRequest vacationRequest)
        {
            if (id != vacationRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacationRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationRequestExists(vacationRequest.Id))
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
            ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId", vacationRequest.ScheduleId);
            return View(vacationRequest);
        }

        // GET: VacationRequests/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vacations == null)
            {
                return NotFound();
            }

            var vacationRequest = await _context.Vacations
                .Include(v => v.Schedules)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationRequest == null)
            {
                return NotFound();
            }

            return View(vacationRequest);
        }

        // POST: VacationRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vacations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vacations'  is null.");
            }
            var vacationRequest = await _context.Vacations.FindAsync(id);
            if (vacationRequest != null)
            {
                _context.Vacations.Remove(vacationRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationRequestExists(int id)
        {
          return (_context.Vacations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
