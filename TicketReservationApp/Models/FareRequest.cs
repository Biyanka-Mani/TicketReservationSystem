using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class FareRequest
    {
        public  List<RouteModel> routes {  get; set; }
        [Required(ErrorMessage = "Please enter the fare.")]
        [Range(20, double.MaxValue, ErrorMessage = "Fare must be a non-negative value and Ranges from 20.")]
        public decimal Fare { get; set; }
      
    }
}
