using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Services
{

    public class BookingServices : IBookingServices
    {
        private readonly TicketReservationDBContext _ticketReservationDBContext;
        private readonly IDataMappingAndAssociationHandler Services;
        public BookingServices(TicketReservationDBContext ticketReservationDBContext, IDataMappingAndAssociationHandler _Services)
        {
            _ticketReservationDBContext = ticketReservationDBContext;
            Services = _Services;

        }
        public async Task<BookingDto> ReserveTickets(Schedule schedule, Route route, BookingDto bookingDto)
        {
            decimal fare = 0;
            if (schedule.AvaliablityOfSeats >= bookingDto.NumberOfTickets)
            {
                fare = CalculateFare(route, bookingDto.NumberOfTickets);
                var booking = new Booking
                {
                    ScheduleId = schedule.Id,
                    TicketsBooked = bookingDto.NumberOfTickets,
                    ModeOfTransport = bookingDto.ModeOfTransport,
                    UserId = bookingDto.UserId,
                    TotalAmountPaid = fare,
                    BookingDate = DateTime.Today,
                    BookingStatus = "Booked",
                };
                _ticketReservationDBContext.Bookings.Add(booking);
                _ticketReservationDBContext.SaveChanges();
                UpdateSeatCountBooking(schedule, bookingDto.NumberOfTickets);
                BookingDto reservation = Services.MapBookingEntityToDto(booking);
                return reservation;
            }
            return new BookingDto();
        }
        public async Task<List<BookingDto>> GetUserBookings(int userId)
        {

            var user =await _ticketReservationDBContext.AppUsers
               .Include(u => u.BookingsOfUser)
            .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var bookingDtos = user.BookingsOfUser.Select(b => Services.MapBookingEntityToDto(b)).ToList();
                return bookingDtos;
            }
            return null;
        }
        public decimal CalculateFare(Route route, int numberOfTickets)
        {
            return route.BaseFare * numberOfTickets;
        }
        private bool UpdateSeatCountBooking(Schedule schedule, int numberOfTickets)
        {
            if (schedule.AvaliablityOfSeats - numberOfTickets >= 0)
            {
                schedule.AvaliablityOfSeats -= numberOfTickets;

                if (schedule.AvaliablityOfSeats == 0)
                {
                    schedule.ScheduleStatusEnum = StatusEnum.BookingClosed;
                }
                _ticketReservationDBContext.Schedules.Update(schedule);
                _ticketReservationDBContext.SaveChanges();

                return true;
            }

            return false;
        }
        private async Task<bool> UpdateSeatCountCancellation(int scheduleid, int numberOfTickets)
        {
            Schedule schedule = _ticketReservationDBContext.Schedules
            .FirstOrDefault(x => x.Id == scheduleid);
            Vehicle vehicle = _ticketReservationDBContext.Vehicles.FirstOrDefault(X => X.Id == schedule.VehicleId);
            if (schedule != null && schedule.AvaliablityOfSeats + numberOfTickets <= vehicle.Capacity)
            {

                schedule.AvaliablityOfSeats += numberOfTickets;
                schedule.ScheduleStatusEnum = StatusEnum.Scheduled;
                _ticketReservationDBContext.Schedules.Update(schedule);
                _ticketReservationDBContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> CancelBooking(int bookingId)
        {
            var booking = _ticketReservationDBContext.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                return false;
            }
            if (booking.BookingStatus != "Booked")
            {
                return false;//for Expiry and cancellation
            }
            booking.BookingStatus = "Cancelled";
            booking.TotalAmountPaid = 0;
            await _ticketReservationDBContext.SaveChangesAsync();

            if (await UpdateSeatCountCancellation(booking.ScheduleId, booking.TicketsBooked))
            {
                await _ticketReservationDBContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteBookings(List<BookingDto> bookings)
        {
            try
            {
                foreach (BookingDto booking in bookings)
                {
                    var bookingId = booking.Id;
                    var bookingEntity = await _ticketReservationDBContext.Bookings.FindAsync(bookingId);
                    if (bookingEntity != null)
                    {
                        _ticketReservationDBContext.Bookings.Remove(bookingEntity);
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
        public List<BookingDto> GetBookingswithScheduleId(int ScheduleId)
        {
            try
            {
                var bookings = _ticketReservationDBContext.Bookings
                    .Where(b => b.ScheduleId == ScheduleId)
                    .Select(b => Services.MapBookingEntityToDto(b))
                    .ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                return new List<BookingDto>();
            }
        }
        public async Task<bool> CancelBookingWithScheduleId(int scheduleId)
        {
            try
            {
                var bookings = _ticketReservationDBContext.Bookings.Where(u => u.ScheduleId == scheduleId);
                Schedule schedule = _ticketReservationDBContext.Schedules.FirstOrDefault(u => u.Id == scheduleId);
                if (schedule != null)
                {
                    schedule.ScheduleStatusEnum = Entites.Models.StatusEnum.Canceled;
                    schedule.IsActiveSchedule = false;
                    await _ticketReservationDBContext.SaveChangesAsync();
                }
                if (bookings.Any())
                {
                    foreach (var booking in bookings)
                    {
                        if (booking.BookingStatus == "Booked")
                        {
                            booking.BookingStatus = "Cancelled";
                            booking.TotalAmountPaid = 0;
                        }

                    }
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
        public async Task<DetailsBooking> GetBookingDetails(int bookingId)
        {
            Booking booking = _ticketReservationDBContext.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            Schedule Schedule = _ticketReservationDBContext.Schedules.FirstOrDefault(b => b.Id == booking.ScheduleId);
            Route Route = _ticketReservationDBContext.Routes.FirstOrDefault(b => b.Id == Schedule.RouteId);
            Terminal DepartureTerminal = _ticketReservationDBContext.Terminals.FirstOrDefault(b => b.Id == Route.DepartureTerminalId);
            Terminal ArrivalTerminal = _ticketReservationDBContext.Terminals.FirstOrDefault(b => b.Id == Route.ArrivalTerminalId);
            Destination Departuredestination = _ticketReservationDBContext.Destinations.FirstOrDefault(b => b.Id == Route.DepatureDestinationId);
            Destination Arrivaldestination = _ticketReservationDBContext.Destinations.FirstOrDefault(b => b.Id == Route.ArrivalDestinationId);

            if (booking != null)
            {
                var Details = new DetailsBooking
                {
                    BookingId = bookingId,
                    JourneyRoute = Route.RouteName,
                    DepartureTime = Schedule.DepartureTime,
                    ArrivalTime = Schedule.ArrivalTime,
                    NoOfTicketsBooked = booking.TicketsBooked,
                    Amount = booking.TotalAmountPaid,
                    DepartureDestination = Departuredestination.Destination_name,
                    depatureTerminal = DepartureTerminal.TerminalName,
                    ArrivalDestination = Arrivaldestination.Destination_name,
                    arrivalTerminal = ArrivalTerminal.TerminalName,
                    Status = booking.BookingStatus

                };
                return Details;
            }
            return null;
        }
        public async Task ChangeStatusBookings(int id)
        {

            Schedule schedule = _ticketReservationDBContext.Schedules.Find(id);
            schedule.ScheduleStatusEnum = Entites.Models.StatusEnum.Canceled;
            schedule.IsActiveSchedule = false;
            _ticketReservationDBContext.SaveChanges();
            List<BookingDto>? bookings = Services.GetAllAssoiciatedBookings(id);
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
        //consider only past 15 days
        public List<int> GetMaxBookedScheduleIds()
        {
            try
            {
                var sevenDaysAgo = DateTime.Now.AddDays(-15);

                var result = _ticketReservationDBContext.Bookings
                    .Where(b => b.BookingDate >= sevenDaysAgo)
                    .GroupBy(b => b.ScheduleId)
                    .Where(group => group.Count() >= 2)
                    .Select(group => group.Key)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool IsBookingsBooked(List<BookingDto> bookingDtos)
        {
            foreach (var booking in bookingDtos)
            {
                if (booking.BookingStatus == "Booked")
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> GetBookingsandDelete(List<ScheduleDto> scheduleDtos)
        {
            foreach (var schedule in scheduleDtos)
            {
                List<BookingDto> bookingDtos = Services.GetAllAssoiciatedBookings(schedule.ScheduleId);
                if (bookingDtos.Count > 0)
                {
                    if (IsBookingsBooked(bookingDtos))
                    {
                        return false;
                    }
                    if (!await DeleteBookings(bookingDtos))
                    {
                        return false;
                    }
                }
                return false;
            }
            return true;
        }

    }

}

