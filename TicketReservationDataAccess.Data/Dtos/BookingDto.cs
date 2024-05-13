using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int NumberOfTickets { get; set; }
        public ModeOfTransport ModeOfTransport { get; set; }
        public int UserId { get; set; }
        public string BookingStatus { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount {  get; set; }
    }
}
