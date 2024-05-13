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
    public class AdminServices : IAdminServices
    {
        private readonly TicketReservationDBContext _reservationDBContext;
        private readonly IBookingServices _bookingServices;
        private readonly IScheduleRelatedServices _scheduleRelatedServices;
        private readonly IDataMappingAndAssociationHandler Services;


        public AdminServices(TicketReservationDBContext reservationDBContext, IScheduleRelatedServices scheduleRelatedServices, IBookingServices bookingServices, IDataMappingAndAssociationHandler _Services)
        {
            _reservationDBContext = reservationDBContext;
            _scheduleRelatedServices = scheduleRelatedServices;
            _bookingServices = bookingServices;
            Services = _Services;
        }
        public async Task<bool> SetFare(int RouteId, decimal NewFare)
        {
            try
            {
                var route = _reservationDBContext.Routes.Find(RouteId);
                route.BaseFare = NewFare;
                await _reservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public decimal GetTotalRevenueFromSchedule(int ScheduleId)
        {
            try
            {
                var Bookings = _reservationDBContext.Bookings.Where(A => A.ScheduleId == ScheduleId);
                decimal TotalRevenue = 0;
                foreach (var Booking in Bookings)
                {
                    TotalRevenue = TotalRevenue + Booking.TotalAmountPaid;
                }
                return TotalRevenue;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> CheckRouteStatus(int id, bool status)
        {
            Route route = _reservationDBContext.Routes.Find(id);
            if (route.RouteStatus == status)
            {
                return true;
            }
            if (status == true)
            {
                return true;
            }
            else
            {
                _scheduleRelatedServices.ChangeStatusSchedule(id);
                return false;
            }
        }
        public async Task<bool> CheckTerminalStatus(int id, bool status)
        {
            Terminal terminal = _reservationDBContext.Terminals.Find(id);
            if (terminal.TerminalStatus == status)
            {
                return true;
            }
            if (status == true)
            {
                return true;
            }
            else
            {
                List<RouteDto>? routes =await  Services.GetAllAssoiciatedRoutes(id);
                foreach (RouteDto route in routes)
                {
                    Route route1 = _reservationDBContext.Routes.Find(route.id);
                    route1.RouteStatus = false;
                    _reservationDBContext.SaveChanges();
                    _scheduleRelatedServices.ChangeStatusSchedule(route.id);
                }
                return false;
            }
        }
        public async Task<bool> CheckScheduleStatus(int id, bool status)
        {
            Schedule schedule = _reservationDBContext.Schedules.Find(id);
            if (schedule.IsActiveSchedule == status)
            {
                return true;
            }
            if (status == true)
            {
                return true;
            }
            else
            {
                await _bookingServices.ChangeStatusBookings(id);
                return false;
            }
        }
        public async Task<List<UserIdDto>> GetAllUsers()
        {
            List<AppUser> appUsers = _reservationDBContext.AppUsers
                .Where(user => user.Id != 1)
                .ToList();

            List<UserIdDto> users = appUsers.Select(user => new UserIdDto
            {
                Id = user.Id,
                Username = user.Username
            }).ToList();

            return users;
        }

    }
}