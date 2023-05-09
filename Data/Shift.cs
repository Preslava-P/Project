using System.ComponentModel.DataAnnotations;

namespace ShiftON.Data
{
    public class Shift
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string TypeShift { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedules { get; set; }
    }
}
