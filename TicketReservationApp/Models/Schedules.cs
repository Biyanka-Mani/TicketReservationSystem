using TicketReservation.App.Models.Enums;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;
using ModeOfTransport = TicketReservation.App.Models.Enums.ModeOfTransport;
using StatusEnum = TicketReservation.App.Models.Enums.StatusEnum;
using TimeOfDayEnum = TicketReservation.App.Models.Enums.TimeOfDayEnum;

namespace TicketReservation.App.Models
{
    public class Schedules
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
        public int seatCount { get; set; }
        public List<RouteDto>? routes { get; set; }
        public List<VehicleDto>? vehicles { get; set; }
        public List<Bookings>? bookings { get; set; }
    }
}
