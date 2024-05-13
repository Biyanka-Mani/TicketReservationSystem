using TicketReservationDataAccess.Data.Entites;

namespace TicketReservation.App.Models
{
    public class AddeditRouteModel
    {
        public int Id { get; set; }
        public List<TerminalViewModel>? Terminals { get; set; }
        public List<DestinationViewModel>? destinations { get; set; }
        public RouteViewModel Route { get; set; }
    }
}
