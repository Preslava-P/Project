using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShiftON.Data
{
    public class VacationRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Поискан ден за почивка")]
        public DateTime RequestDay { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedules { get; set; }
    }
}
