using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Entites.Models
{
    public class DetailsBooking
    {
        public int BookingId {  get; set; }
        public string JourneyRoute { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int NoOfTicketsBooked {  get; set; }
        public decimal Amount {  get; set; }
        public string DepartureDestination { get; set; }
        public string depatureTerminal {  get; set; }
        public string ArrivalDestination { get; set; }
        public string arrivalTerminal {  get; set; }
        public string Status {  get; set; }

    }
}
