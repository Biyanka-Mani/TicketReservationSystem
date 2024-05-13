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
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }

        public int TicketsBooked { get; set; }

        public ModeOfTransport ModeOfTransport { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }

        public DateTime BookingDate { get; set; }
        public string BookingStatus { get; set; }
        //booked
        //cancelled
        //expired

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmountPaid { get; set; }
    }
        
}
