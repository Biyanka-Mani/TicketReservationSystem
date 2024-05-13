using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class RouteModel
    {

        public int RouteId { get; set; }

        public string? RouteName { get; set; }
        public decimal fare { get; set; }

        public decimal? TotalRevenue {  get; set; }
    }
}
