using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShiftON.Data
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<VacationRequest> Vacations { get; set; }
    }
}