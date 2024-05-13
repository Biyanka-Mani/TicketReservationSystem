using TicketReservationDataAccess.Data.Dtos;

namespace TicketReservation.App.Models
{
    public class UserModel
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public List<BookingDto>? Bookings { get; set; }
    }
}
