using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShiftON.Data
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Ден от седмицата")]
        public DateTime DayOfWeek { get; set; }

        [Required]
        [DisplayName("Служители")]
        public string CustomerId { get; set; }
        public Customer Customers { get; set; }

        public ICollection<Shift> Shifts { get; set; }
        public ICollection<VacationRequest> VacationRequests { get; set; }
    }
}
