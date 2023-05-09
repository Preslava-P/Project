using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShiftON.Data
{
    public class Customer : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        [DisplayName("Име")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        public ICollection<VacationRequest> Vacations { get; set; }

    }
}
