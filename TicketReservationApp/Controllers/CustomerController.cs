using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketReservation.App.Models;
using TicketReservation.App.Services;
using TicketReservationDataAccess.Data.Entites;
using TicketReservation.App.Models.Enums;
using TicketReservationDataAccess.Data.Entites.Models;
using ModeOfTransport = TicketReservation.App.Models.Enums.ModeOfTransport;
using TimeOfDayEnum = TicketReservation.App.Models.Enums.TimeOfDayEnum;


namespace TicketReservation.App.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerServices customerServices;
        private readonly CommonServices commonServices;
        
        public CustomerController(CustomerServices _customerServices, CommonServices commonServices)
        {
            this.customerServices = _customerServices;
            this.commonServices = commonServices;
        }

        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]
        public async Task< IActionResult> Index()
        {
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<DestinationViewModel> destinations = await commonServices.GetAllDestinations();
            return View(destinations);
        }
        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]
        public async Task<IActionResult> SeeSchedules(int SelectedStartingPointId, int SelectedEndPointId)
        {
            var schedules = await customerServices.SeeSchedules(SelectedStartingPointId, SelectedEndPointId);
            if (schedules.Count > 0)
            {
                return View("SchdeulesForARoute", schedules);
            }
            TempData["ErrorMessage"] = "Routes Not Avaliable!";
            return RedirectToAction("Index");
        }
        

        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdString, out int userId))
            {
                var Bookings = await customerServices.GetAllUserBookings(userId);
                if (Bookings != null && Bookings.Any())
                {
                    return View(Bookings);
                }
            }
            return View("NoBookingExistForUser");

        }
        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]
        public async Task<IActionResult> Details(int bookingId)
        {
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                var BookingDetails = await customerServices.GetDetailsOfBooking(bookingId);
                if (BookingDetails != null)
                {
                    return View(BookingDetails);
                }
            }
            //
            TempData["ErrorMessage"] = "Booking Details Not Avaliable!";
            return RedirectToAction("MyBookings", "Customer");
        }

        [Authorize(AuthenticationSchemes = "UserCookies")]
        public async Task<IActionResult> SearchRoutes()
        {
            List<DestinationViewModel> destinations = await commonServices.GetAllDestinations();
            return View(destinations);
        }
       

        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]
        public async Task<IActionResult> SearchRoutesUser( int SelectedStartingPointId ,int SelectedEndPointId , int NumberOfPassengers)
        {
            var schedules = await customerServices.GetAvalaibleScheduleForRoute(SelectedStartingPointId, SelectedEndPointId, NumberOfPassengers);

            if (schedules.Count > 0)
            {
                ViewBag.SelectedStartingPointId = SelectedStartingPointId;
                ViewBag.SelectedEndPointId = SelectedEndPointId;
                ViewBag.NumberOfPassengers = NumberOfPassengers;
                ViewBag.passengercount = NumberOfPassengers;
                return View("Schedules", schedules);
            }

            TempData["ErrorMessage"] = "Routes Not Avaliable!";
            return RedirectToAction("SearchRoutes");
        }
        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]

        public async Task<IActionResult> ScheduleFiltering(ModeOfTransport modeOfTransport, TimeOfDayEnum timeOfDayEnum, int SelectedStartingPointId, int SelectedEndPointId, int NumberOfPassengers)
        {
            var searchResults = await customerServices.FilterSchedules(timeOfDayEnum, modeOfTransport, SelectedStartingPointId, SelectedEndPointId, NumberOfPassengers);
            ViewBag.passengercount = NumberOfPassengers;
            return PartialView("_partialScheduleFiltering",searchResults);

        }

        [Authorize(AuthenticationSchemes = "UserCookies")]
        public async Task<IActionResult> ConfirmTicket(int scheduleId, int passengercount)
        {
            ViewBag.scheduleId = scheduleId;
            ViewBag.passengercount = passengercount;
           

            decimal fare = await customerServices.FareCalculation(scheduleId, passengercount);
            ViewBag.fare = fare;
            return View();
        }
        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpPost]
        public async Task<IActionResult> Booking(int Scheduleid, int passengercount)
        {
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                BookingRequest bookingRequest = new BookingRequest();
                bookingRequest.UserId = userId;
                bookingRequest.scheduleid = Scheduleid;
                bookingRequest.NoofTickets = passengercount;
                Bookings Booking = await customerServices.MakeReservation(bookingRequest);
                ViewBag.Booking = Booking;
                return View();
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("Index", "Customer");

        }

        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpPost]
        public async Task<IActionResult> CancelConfirmation(int Bookingid)
        {
            ViewBag.BookingId = Bookingid;
            return View();
        }

        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int Bookingid)
        {
            if (await commonServices.CancelBooking(Bookingid))
            {

                TempData["SuccessMessage"] = "Booking Cancelled!";
                return RedirectToAction("MyBookings", "Customer");
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("MyBookings", "Customer");
        }
        [Authorize(AuthenticationSchemes = "UserCookies")]
        [HttpGet]
        public async Task<IActionResult> EditProfileView()
        {
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdString, out int userId))
            {
                UserViewModel user = await customerServices.GetProfile(userId);
                return View("EditProfile", user);
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("Index", "Customer");
        }
        [Authorize(AuthenticationSchemes = "UserCookies")]
        public async Task<IActionResult> EditUser(EditUserViewModel userViewModel)
        {
            
            bool Edited = await customerServices.EditProfile(userViewModel);
            if (Edited)
            {
                TempData["SuccessMessage"] = "Profile Updated Successfully!";
                return RedirectToAction("EditProfileView", "Customer");

            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("EditProfileView", "Customer");

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "UserCookies")]
        public IActionResult ChangePasswordView()
        {
            return View();
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "UserCookies")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user
                string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                {
                    return NotFound($"Unable to load user with ID .");
                }
                var result = await customerServices.ChangePasswordAsync(userId,model);
                if (result)
                {
                    TempData["SuccessMessage"] = "Profile Updated Successfully!";
                    return RedirectToAction("ChangePasswordView", "Customer");
                }
                else
                {
                    TempData["ErrorMessage"] = "Old Password Wrong";
                    return RedirectToAction("ChangePasswordView", "Customer");
                }
            }
            return View("ChangePasswordView", model);


        }
        
    }
}
