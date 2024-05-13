using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IBookingServices
    {
        decimal CalculateFare(Route route, int numberOfTickets);
        Task<bool> CancelBooking(int bookingId);
        Task<bool> CancelBookingWithScheduleId(int scheduleId);
        Task ChangeStatusBookings(int id);
        Task<bool> DeleteBookings(List<BookingDto> bookings);
        Task<DetailsBooking> GetBookingDetails(int bookingId);
        Task<bool> GetBookingsandDelete(List<ScheduleDto> scheduleDtos);
        List<BookingDto> GetBookingswithScheduleId(int ScheduleId);
        List<int> GetMaxBookedScheduleIds();
       Task<List<BookingDto>> GetUserBookings(int userId);
        bool IsBookingsBooked(List<BookingDto> bookingDtos);
        Task<BookingDto> ReserveTickets(Schedule schedule, Route route, BookingDto bookingDto);
    }
}