using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public class ScheduleRelatedServices : IScheduleRelatedServices
    {
        private readonly TicketReservationDBContext _ticketReservationDBContext;
        private readonly IRouteTerminalServices _routeTerminalServices;
        private readonly IDataMappingAndAssociationHandler Services;

        public ScheduleRelatedServices(TicketReservationDBContext ticketReservationDBContext, IDataMappingAndAssociationHandler _Services, IRouteTerminalServices routeTerminalServices)
        {
            _ticketReservationDBContext = ticketReservationDBContext;
            Services = _Services;
            _routeTerminalServices = routeTerminalServices;

        }
        public Schedule GetScheduleById(int scheduleid)
        {
            Schedule schedule = _ticketReservationDBContext.Schedules.FirstOrDefault(x => x.Id == scheduleid);
            return schedule;
        }
        public async Task<ScheduleDto> GetSchedule(int scheduleid)
        {
            var schedule = _ticketReservationDBContext.Schedules.FirstOrDefault(x => x.Id == scheduleid);
            ScheduleDto scheduleDto = Services.MapScheduleEntityToDto(schedule);
            return scheduleDto;
        }
        public async Task<bool> DeleteSchedules(List<ScheduleDto> schedules)
        {
            try
            {
                foreach (ScheduleDto schedule in schedules)
                {
                    var scheduleid = schedule.ScheduleId;
                    var scheduleEntity = await _ticketReservationDBContext.Schedules.FindAsync(scheduleid);
                    if (scheduleEntity != null)
                    {
                        _ticketReservationDBContext.Schedules.Remove(scheduleEntity);
                    }
                }
                await _ticketReservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteSchedule(int scheduleId)
        {
            try
            {
                var schedule = await _ticketReservationDBContext.Schedules.FindAsync(scheduleId);
                if (schedule != null)
                {
                    _ticketReservationDBContext.Schedules.Remove(schedule);
                    await _ticketReservationDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public ICollection<ScheduleDto> GetSchedulesForRoute(int routeId)
        {
            var route = _ticketReservationDBContext.Routes.Include(u => u.Schedules).FirstOrDefault(u => u.Id == routeId);
            if (route != null)
            {
                var ScheduleDto = route.Schedules.Select(b => Services.MapScheduleEntityToDto(b)).ToList();
                return ScheduleDto;
            }
            return null;
        }
        public async Task<bool> AddSchedule(ScheduleDto scheduleDto)
        {
            var newSchedule = new Schedule
            {
                DepartureTime = scheduleDto.DepartureTime,
                ArrivalTime = scheduleDto.ArrivalTime,
                IsActiveSchedule = scheduleDto.IsActiveSchedule,
                ScheduleStatusEnum = scheduleDto.ScheduleStatusEnum,
                TimeOfDay = scheduleDto.TimeOfDay,
                VehicleId = scheduleDto.VehicleId,
                RouteId = scheduleDto.RouteId,
                ModeOfTransport = scheduleDto.modeOfTransport,
                AvaliablityOfSeats = await _routeTerminalServices.GetCapacity(scheduleDto.VehicleId),
            };

            _ticketReservationDBContext.Schedules.Add(newSchedule);
            try
            {
                await _ticketReservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> EditSchedule(int scheduleId, ScheduleDto scheduleDto)
        {
            try
            {
                var existingSchedule = _ticketReservationDBContext.Schedules.FirstOrDefault(s => s.Id == scheduleId);
                if (existingSchedule != null)
                {
                    existingSchedule.DepartureTime = scheduleDto.DepartureTime;
                    existingSchedule.ArrivalTime = scheduleDto.ArrivalTime;
                    existingSchedule.IsActiveSchedule = scheduleDto.IsActiveSchedule;
                    existingSchedule.ScheduleStatusEnum = scheduleDto.ScheduleStatusEnum;
                    existingSchedule.TimeOfDay = scheduleDto.TimeOfDay;
                    existingSchedule.VehicleId = scheduleDto.VehicleId;
                    existingSchedule.RouteId = scheduleDto.RouteId;
                    existingSchedule.ModeOfTransport = scheduleDto.modeOfTransport;

                    await _ticketReservationDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ScheduleAddRequsetDto> ScheduleAddRequset()
        {
            ScheduleAddRequsetDto scheduleAddRequsetDtos = new ScheduleAddRequsetDto();
            scheduleAddRequsetDtos.Route = await _routeTerminalServices.GetAllAvaliableRoutes();
            scheduleAddRequsetDtos.Vechicle = await _routeTerminalServices.GetAllVehicles();
            return scheduleAddRequsetDtos;

        }
        public async Task ChangeStatusSchedule(int id)
        {
            List<ScheduleDto>? schedules =Services.GetAllAssoiciatedSchedules(id);
            foreach (ScheduleDto schedule in schedules)
            {
                Schedule schedule1 = _ticketReservationDBContext.Schedules.Find(schedule.ScheduleId);
                schedule1.ScheduleStatusEnum = Entites.Models.StatusEnum.Canceled;
                schedule1.IsActiveSchedule = false;
                _ticketReservationDBContext.SaveChanges();
                List<BookingDto>? bookings = Services.GetAllAssoiciatedBookings(schedule1.Id);
                if (bookings.Count > 0)
                {
                    foreach (BookingDto booking in bookings)
                    {
                        Booking booking1 = _ticketReservationDBContext.Bookings.Find(booking.Id);
                        booking1.BookingStatus = "Cancelled";
                        _ticketReservationDBContext.SaveChanges();
                    }

                }

            }

        }
        public async Task<List<ScheduleDto>> GetAllSchedules()
        {
            List<Schedule> schedules = _ticketReservationDBContext.Schedules.ToList();
            List<ScheduleDto> result = new List<ScheduleDto>();
            foreach (var schedule in schedules)
            {
                ScheduleDto scheduleDto = Services.MapScheduleEntityToDto(schedule);

                scheduleDto.vehicles = await _routeTerminalServices.GetAllVehicles();
                scheduleDto.routes = await _routeTerminalServices.GetAllroutes();
                result.Add(scheduleDto);
            }
            return result;
        }
        public bool IsSchedulesScheduled(List<ScheduleDto> scheduleDtos)
        {
            foreach (var schedule in scheduleDtos)
            {
                if (schedule.ScheduleStatusEnum == TicketReservationDataAccess.Data.Entites.Models.StatusEnum.Scheduled)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
