using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShiftON.Data;

namespace ShiftON.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Customer> _userManager;

        public SchedulesController(ApplicationDbContext context, UserManager<Customer> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Test()
        {
            var userLoged = await _userManager.GetUserAsync(User);
            var result = await _userManager.AddToRoleAsync(userLoged, "Admin");
            var roles = _userManager.GetRolesAsync(userLoged);
            return Content("OK!");
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Schedules.Include(s => s.Customers);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Customers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public async Task<IActionResult> Create()
        {
            var employee = await _userManager.GetUsersInRoleAsync("User");
            var result = employee.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            }
            ).ToList();
            ViewData["CustomerId"] = result;
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();

        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DayOfWeek,CustomerId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "FirstName", "LastName", schedule.CustomerId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "FirstName", "LastName", schedule.CustomerId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfWeek,CustomerId")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "FirstName", "LastName", schedule.CustomerId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Customers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
