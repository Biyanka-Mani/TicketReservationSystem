using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IDataMappingAndAssociationHandler
    {
        List<BookingDto> GetAllAssoiciatedBookings(int ScheduleiD);
        List<RouteDto> GetAllAssoiciatedRoute(int destinationid);
        Task<List<RouteDto>> GetAllAssoiciatedRoutes(int terminalid);
        List<ScheduleDto> GetAllAssoiciatedSchedules(int routeId);
        List<ScheduleDto> GetAllAssoiciatedSchedulesForVehicle(int vehicleId);
        BookingDto MapBookingEntityToDto(Booking booking);
        RouteDto MapRouteEntityToDto(Route route);
        ScheduleDto MapScheduleEntityToDto(Schedule schedule);
        TerminalDto MapTermianlEntityToDto(Terminal terminal);
    }
}