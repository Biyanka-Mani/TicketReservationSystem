using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class DestinationViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Destination name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only letters and spaces are allowed")]
        public string Destination_name { get; set; }
    }
}
