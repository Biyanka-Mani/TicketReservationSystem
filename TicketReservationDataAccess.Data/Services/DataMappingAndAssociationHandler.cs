using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public class DataMappingAndAssociationHandler : IDataMappingAndAssociationHandler
    {
        private readonly TicketReservationDBContext _ticketReservationDBContext;
        public DataMappingAndAssociationHandler(TicketReservationDBContext ticketReservationDBContext)
        {
            _ticketReservationDBContext = ticketReservationDBContext;
        }
        public async Task<List<RouteDto>> GetAllAssoiciatedRoutes(int terminalid)
        {
            var associatedRoutes = _ticketReservationDBContext.Routes
            .Where(u => u.ArrivalTerminalId == terminalid || u.DepartureTerminalId == terminalid)
            .ToList();
            List<RouteDto> routes = new List<RouteDto>();
            foreach (var associatedRoute in associatedRoutes)
            {
                RouteDto route = MapRouteEntityToDto(associatedRoute);
                routes.Add(route);
            }


            return routes;
        }
        public List<RouteDto> GetAllAssoiciatedRoute(int destinationid)
        {
            var associatedRoutes = _ticketReservationDBContext.Routes
            .Where(u => u.ArrivalDestinationId == destinationid || u.DepatureDestinationId == destinationid)
            .ToList();
            List<RouteDto> routes = new List<RouteDto>();
            foreach (var associatedRoute in associatedRoutes)
            {
                RouteDto route = MapRouteEntityToDto(associatedRoute);
                routes.Add(route);
            }


            return routes;
        }
        public List<BookingDto> GetAllAssoiciatedBookings(int ScheduleiD)
        {
            var schedule = _ticketReservationDBContext.Schedules.Include(u => u.Bookings).FirstOrDefault(u => u.Id == ScheduleiD);
            if (schedule != null)
            {
                var bookingDto = schedule.Bookings.Select(b => MapBookingEntityToDto(b)).ToList();
                return bookingDto;
            }
            return null;
        }
        public List<ScheduleDto> GetAllAssoiciatedSchedules(int routeId)
        {
            var route = _ticketReservationDBContext.Routes.Include(u => u.Schedules).FirstOrDefault(u => u.Id == routeId);
            if (route != null)
            {
                var scheduleDto = route.Schedules.Select(b => MapScheduleEntityToDto(b)).ToList();
                return scheduleDto;
            }
            return null;
        }
        public List<ScheduleDto> GetAllAssoiciatedSchedulesForVehicle(int vehicleId)
        {
            var schedules = _ticketReservationDBContext.Schedules
                                .Where(u => u.VehicleId == vehicleId)
                                .ToList();

            if (schedules != null && schedules.Any())
            {
                var scheduleDtos = schedules.Select(schedule => MapScheduleEntityToDto(schedule)).ToList();
                return scheduleDtos;
            }

            return null;
        }
        public BookingDto MapBookingEntityToDto(Booking booking)
        {

            return new BookingDto
            {
                Id = booking.BookingId,
                ScheduleId = booking.ScheduleId,
                BookingStatus = booking.BookingStatus,
                ModeOfTransport = booking.ModeOfTransport,
                NumberOfTickets = booking.TicketsBooked,
                Created = booking.BookingDate,
                Amount = booking.TotalAmountPaid

            };
        }
        public ScheduleDto MapScheduleEntityToDto(Schedule schedule)
        {
            return new ScheduleDto()
            {
                ScheduleId = schedule.Id,
                DepartureTime = schedule.DepartureTime,
                ArrivalTime = schedule.ArrivalTime,
                IsActiveSchedule = schedule.IsActiveSchedule,
                ScheduleStatusEnum = schedule.ScheduleStatusEnum,
                modeOfTransport = schedule.ModeOfTransport,
                TimeOfDay = schedule.TimeOfDay,
                VehicleId = schedule.VehicleId,
                RouteId = schedule.RouteId,
                seatCount = schedule.AvaliablityOfSeats,
                bookings = GetAllAssoiciatedBookings(schedule.Id)

            };

        }
        public RouteDto MapRouteEntityToDto(Route route)
        {
            return new RouteDto()
            {
                id = route.Id,
                RouteName = route.RouteName,
                ArrivalTerminalId = route.ArrivalTerminalId,
                RouteStatus = route.RouteStatus,
                DepartureTerminalId = route.DepartureTerminalId,
                DepartureDestinationId = route.DepatureDestinationId,
                ArrivalDestinationId = route.ArrivalDestinationId,
                BaseFare = route.BaseFare,

            };
        }
        public TerminalDto MapTermianlEntityToDto(Terminal terminal)
        {
            return new TerminalDto()
            {
                Id = terminal.Id,
                TerminalName = terminal.TerminalName,
                TerminalStatus = terminal.TerminalStatus,
                Location = terminal.Location,
            };
        }

    }
}
