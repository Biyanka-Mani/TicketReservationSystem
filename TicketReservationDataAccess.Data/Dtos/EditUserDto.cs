using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Dtos
{
    public  class EditUserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 10 characters.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name should only contain letters and spaces.")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name should only contain letters and spaces.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone number should only contain numbers.")]
        [StringLength(10)]
        public string? Phone { get; set; }

        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int? Age { get; set; }

        public string? BloodGroup { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]{10}$", ErrorMessage = "Invalid Identity Card Number format.")]
        public string? IdentityCardNumber { get; set; }

        public string? SeatPreference { get; set; }
    }
}
