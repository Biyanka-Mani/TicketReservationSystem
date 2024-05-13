using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservation.App.Models
{
    public class ScheduleAddRequset
    {
        public IEnumerable<RouteDto> Route { get; set; }
        public IEnumerable<VehicleDto> Vechicle { get; set; }

    }
}
