using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Dtos
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool IsActiveSchedule { get; set; }
        public StatusEnum ScheduleStatusEnum { get; set; }
        public ModeOfTransport modeOfTransport { get; set; }

        public TimeOfDayEnum TimeOfDay { get; set; }
        public int VehicleId { get; set; }
        public int RouteId { get; set; }
        public int seatCount {  get; set; }
        public List<VehicleDto>? vehicles { get; set; }
        public List<RouteDto>? routes { get; set; }
        public List<BookingDto>? bookings { get; set; }
    }
}
