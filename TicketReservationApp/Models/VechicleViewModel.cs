using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class VechicleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vehicle Name is required")]
        public string VehicleName { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Capacity is required")]
        [Range(5, int.MaxValue, ErrorMessage = "Capacity must be greater than 5")]
        public int Capacity { get; set; }
    }
}
