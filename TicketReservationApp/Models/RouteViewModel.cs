using System.ComponentModel.DataAnnotations;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservation.App.Models
{
    public class RouteViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "RouteName is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "RouteName must be between 10 and 100 characters.")]
        public string RouteName { get; set; }

        [Required(ErrorMessage = "DepartureDestination is required.")]
        public int DepartureDestinationId { get; set; }

        [Required(ErrorMessage = "ArrivalDestination is required.")]
        public int  ArrivalDestinationId { get; set; }

        [Required(ErrorMessage = "BaseFare is required.")]
        [Range(20, 10000, ErrorMessage = "BaseFare must be between 20 and 10000.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "BaseFare must be a valid number.")]
        public decimal BaseFare { get; set; }

        public bool RouteStatus { get; set; }

        [Required(ErrorMessage = "DepartureTerminal is required.")]
        public int DepartureTerminalId { get; set; }

        [Required(ErrorMessage = "ArrivalTerminal is required.")]
        public int ArrivalTerminalId { get; set; }
        public List<destinationDto>? destinations { get; set; }
        public List<TerminalDto>? terminals { get; set; }

    }

}
