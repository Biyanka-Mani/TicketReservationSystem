using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Entites
{
    public class AppUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }

        public string Userpassword { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }

        public int? Age { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }
        public bool UserRole { get; set; } = true;
        //true - user

        public string? BloodGroup { get; set; }

        [MaxLength(50)]
        public string? IdentityCardNumber { get; set; }


        [MaxLength(50)]
        public string? SeatPreference { get; set; }

        public ICollection<Booking>? BookingsOfUser { get; set; } 
    }
}
