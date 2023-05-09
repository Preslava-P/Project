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
    public class ShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shifts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Shifts.Include(s => s.Schedules);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Shifts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Shifts == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Schedules)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shifts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId");
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeShift,ScheduleId")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                shift.Id = Guid.NewGuid();
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId", shift.ScheduleId);
            return View(shift);
        }

        // GET: Shifts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Shifts == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId", shift.ScheduleId);
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TypeShift,ScheduleId")] Shift shift)
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.Id))
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
            //ViewData["ScheduleId"] = new SelectList(_context.Schedules, "Id", "CustomerId", shift.ScheduleId);
            return View(shift);
        }

        // GET: Shifts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Shifts == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Schedules)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Shifts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Shifts'  is null.");
            }
            var shift = await _context.Shifts.FindAsync(id);
            if (shift != null)
            {
                _context.Shifts.Remove(shift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(Guid id)
        {
          return (_context.Shifts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
