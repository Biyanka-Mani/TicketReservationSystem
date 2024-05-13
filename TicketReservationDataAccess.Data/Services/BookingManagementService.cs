using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Services
{
    public class BookingManagementService
    {
        private readonly TicketReservationDBContext _reservationDBContext;
        public BookingManagementService(TicketReservationDBContext reservationDBContext)
        {
            _reservationDBContext = reservationDBContext;
        }
       
        public async Task MakeBookingsExpiray()
        {
            List<Schedule>? schedules = _reservationDBContext.Schedules.ToList();

            if (schedules.Count == 0)
            {
                return;
            }

            foreach (var schedule in schedules)
            {
                if (schedule.DepartureTime < DateTime.Now)
                {
                    schedule.ScheduleStatusEnum = StatusEnum.BookingClosed;
                    schedule.IsActiveSchedule = false;

                    List<Booking> bookings = _reservationDBContext.Bookings.Where(u => u.ScheduleId == schedule.Id).ToList();
                    foreach (var booking in bookings)
                    {
                        booking.BookingStatus = "Expired";
                    }
                }
            }
            await _reservationDBContext.SaveChangesAsync();
        }
        public async Task MakeBookingsCancelled()
        {
            var schedules = _reservationDBContext.Schedules.Where(u => u.ScheduleStatusEnum == StatusEnum.Canceled).ToList();
            if (schedules.Count == 0)
            { 
                return; 
            }
            foreach (var schedule in schedules)
            {

                var bookings = _reservationDBContext.Bookings.Where(u => u.ScheduleId == schedule.Id).ToList();
                if(bookings.Count == 0)
                { 
                    return; 
                }
                foreach (var booking in bookings)
                {
                    booking.BookingStatus = "Cancelled";
                    booking.TotalAmountPaid = 0;
                }
                await _reservationDBContext.SaveChangesAsync();
            }


        }
        public async Task MakingSchedulesInactive(int id)
        {
            Schedule schedule = _reservationDBContext.Schedules.FirstOrDefault(u => u.Id == id);
            if (schedule.ScheduleStatusEnum == Entites.Models.StatusEnum.Canceled)
            {
                schedule.IsActiveSchedule = false;
                _reservationDBContext.SaveChanges();
            }
        }

    }
}
