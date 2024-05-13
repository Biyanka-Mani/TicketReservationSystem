using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IScheduleRelatedServices
    {
        Task<bool> AddSchedule(ScheduleDto scheduleDto);
        Task ChangeStatusSchedule(int id);
        Task<bool> DeleteSchedule(int scheduleId);
        Task<bool> DeleteSchedules(List<ScheduleDto> schedules);
        Task<bool> EditSchedule(int scheduleId, ScheduleDto scheduleDto);
        Task<List<ScheduleDto>> GetAllSchedules();
        Task<ScheduleDto> GetSchedule(int scheduleid);
        Schedule GetScheduleById(int scheduleid);
        ICollection<ScheduleDto> GetSchedulesForRoute(int routeId);
        bool IsSchedulesScheduled(List<ScheduleDto> scheduleDtos);
        Task<ScheduleAddRequsetDto> ScheduleAddRequset();
    }
}