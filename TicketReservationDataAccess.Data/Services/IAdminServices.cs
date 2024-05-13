using TicketReservationDataAccess.Data.Dtos;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IAdminServices
    {
        Task<bool> CheckRouteStatus(int id, bool status);
        Task<bool> CheckScheduleStatus(int id, bool status);
        Task<bool> CheckTerminalStatus(int id, bool status);
        Task<List<UserIdDto>> GetAllUsers();
        decimal GetTotalRevenueFromSchedule(int ScheduleId);
        Task<bool> SetFare(int RouteId, decimal NewFare);
    }
}