using TicketReservationDataAccess.Data.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketReservationDataAccess.Data.Services;
using System.Net.Mime;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Core;
using Microsoft.AspNetCore.Identity;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IValidationServices _ValidationServices;
        private readonly IRouteTerminalServices _routeTerminalServices;
        private readonly IBookingServices _bookingServices;
        private readonly BookingManagementService _bookingManagementService;
        private readonly IScheduleRelatedServices _scheduleRelatedServices;
       
        public UserController(IUserServices userServices,IValidationServices ValidationServices,IRouteTerminalServices routeTerminalServices, IBookingServices bookingServices, BookingManagementService _BookingManagementService, IScheduleRelatedServices scheduleRelatedServices)
        {
            _userServices = userServices;
            _ValidationServices = ValidationServices;
            _routeTerminalServices = routeTerminalServices;
            _bookingServices = bookingServices;
            _bookingManagementService = _BookingManagementService;
            _scheduleRelatedServices = scheduleRelatedServices;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            if (_ValidationServices.IsValidUsername(user.Username) || _ValidationServices.IsValidPassword(user.Userpassword))
            {
                return BadRequest("UserName/Password Is Invalid");
            }
            if (user.Age < 18 || user.Age > 120)
            {
                return BadRequest("Invalid Age");
            }
            if (_ValidationServices.IsValidPhonenumber(user.Phone))
            {
                return BadRequest("Invalid Phonenumber");
            }

            if (!await _ValidationServices.IsUsernameUnique(user.Username))
            {
                return BadRequest("Username Taken, Please Change");
            }
            if (_ValidationServices.DoesUserExist(user.Phone))
            {
                return BadRequest("Phone Number Exist");

            }
            await _userServices.CreateAccount(user);
            return Ok();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            LoginDto isAuthenticated = await _ValidationServices.AuthenticateUser(loginDto);
            
            if (isAuthenticated!= null)
            {

                return Ok(isAuthenticated);
                
            }
            return Unauthorized(new { Message = "Authentication failed" });
           
        }

        [HttpPost("VerifyAccount")]
        public async Task<IActionResult>AccountVerification(ResetPasswordDto resetPasswordDto)
        {
            UserIdDto user = await _ValidationServices.VerifyAccount(resetPasswordDto);

            return Ok(user);

        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string UserName, [FromQuery] string Password)
        {
            AppUser user = await _userServices.GetUserByUsername(UserName);
            if (user == null)
            {
                return Ok(false);
            }
            bool changed = _userServices.ChangePassword(user, Password);
            return Ok(changed);
        }
        [HttpGet("user/EditProfile/{userid}")]
        public async Task<IActionResult> GetProfile(int userid)
        {
            UserDto userDto = await _userServices.GetProfile(userid);
            if (userDto != null)
            {
                return Ok(userDto);
            }
            return NotFound();
        }

        [HttpPost("profileEdit")]
        public async Task<IActionResult> UpdateUserProfile(EditUserDto userProfileUpdateDto)
        {
            bool updateResult = await _userServices.UpdateUserProfileAsync(userProfileUpdateDto.Id, userProfileUpdateDto);

            if (updateResult)
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpPost("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id,ChangePasswordModel model)
        {
            try
            {
                AppUser appUser = await _userServices.GetUserByUserId(id);
                if( _ValidationServices.VerifyPassword(appUser, model.OldPassword))
                {
                    
                    _userServices.ChangePassword(appUser, model.NewPassword);
                    return Ok(true);

                }
                return Ok(false);
                
            }
            catch (Exception )
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("routes")]
        public async Task<IActionResult> GetAllAvailableRoutes()
        {
            try
            {
                var routes = _routeTerminalServices.GetAllAvaliableRoutes();
                if (routes != null)
                {
                    return Ok(routes);
                }
                return NotFound("No routes available.");
            }
            catch (Exception )
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetAllDestinations")]
        public async Task<IActionResult> GetAllDestinations()
        {
            try
            {
                var Destinations = _routeTerminalServices.GetAllDestinations().Result;
                if (Destinations != null)
                {
                    return Ok(Destinations);
                }
                return NotFound();
            }
            catch (Exception )
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("SeeSchedules/{SelectedStartingPointId}/{SelectedEndPointId}")]
        public async Task<IActionResult> SeeSchedules(int SelectedStartingPointId, int SelectedEndPointId)
        {
            await _bookingManagementService.MakeBookingsExpiray();

            try
            {
                int routeId = await _routeTerminalServices.GetRouteIdByLocation(SelectedStartingPointId, SelectedEndPointId);
                if (routeId == 0)
                {
                    return NotFound("No routes found for the specified locations.");
                }
                var schedules = _scheduleRelatedServices.GetSchedulesForRoute(routeId);
                if (schedules.Count == 0)
                {
                    return NotFound("No Schedules found for the specified Route");
                }
                List<ScheduleDto> scheduleslist = new List<ScheduleDto>();
                foreach (var schedule in schedules)
                {
                    _bookingManagementService.MakingSchedulesInactive(schedule.ScheduleId);
                    if (schedule.IsActiveSchedule == true)
                    {
                        scheduleslist.Add(schedule);
                    }

                }

                return Ok(scheduleslist);


            }
            catch (Exception)
            {
               
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("routes/{startingFrom}/{goingTo}/{passengerCount}")]
        public async Task<IActionResult> GetRoutesByLocation(int startingFrom,int goingTo,int passengerCount)
        {
            await _bookingManagementService.MakeBookingsExpiray();
            
            try
            {
                int routeId = await _routeTerminalServices.GetRouteIdByLocation(startingFrom, goingTo);
                if(routeId==0)
                {
                    return NotFound("No routes found for the specified locations."); 
                }
                var schedules = _scheduleRelatedServices.GetSchedulesForRoute(routeId);
                if(schedules.Count ==0) 
                { 
                    return NotFound("No Schedules found for the specified Route"); 
                }
                List<ScheduleDto> scheduleslist= new List<ScheduleDto>();
                foreach (var schedule in schedules)
                {
                    if (schedule.seatCount >= passengerCount)
                    {
                        _bookingManagementService.MakingSchedulesInactive(schedule.ScheduleId);
                        scheduleslist.Add(schedule);
                    }
                    
                }
                if (scheduleslist.Count == 0)
                {
                    return NotFound("No schedules available for the specified passenger count.");
                }

                return Ok(scheduleslist);

                
            }
            catch (Exception )
            {
               
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("schedules/{routeId}")]
        public async Task<IActionResult> GetSchedulesByRouteId(int routeId)
        {
            try
            {
                await _bookingManagementService.MakeBookingsExpiray();
                
                var schedules =_scheduleRelatedServices.GetSchedulesForRoute(routeId);
                if (schedules != null)
                {
                    return Ok(schedules);
                }
                return NotFound(new { Message = "Schedules not found for the given route ID" });
            }
            catch (Exception )
            {
                
                return StatusCode(500, new { Message = "An error occurred while processing the request" });
            }
        }

        [HttpPost("ReserveTickets/{scheduleId}")]
        public async Task<IActionResult> ReserveTickets(BookRequestDto bookRequestDto)
        {
            try
            {
                await _bookingManagementService.MakeBookingsCancelled();                        ;

                Schedule schedule= _scheduleRelatedServices.GetScheduleById(bookRequestDto.scheduleid);
                if (schedule == null)
                {
                    return NotFound();
                }
                var route = _routeTerminalServices.GetRouteById(schedule.RouteId);
                if (route == null)
                {
                    return NotFound();
                }
                BookingDto bookingDto = new BookingDto();
                bookingDto.ScheduleId = schedule.Id;
                bookingDto.UserId=bookRequestDto.UserId;
                bookingDto.NumberOfTickets = bookRequestDto.NoofTickets;
                bookingDto.ModeOfTransport=schedule.ModeOfTransport;

                var reservation = await _bookingServices.ReserveTickets(schedule,route, bookingDto);

                if (reservation.Id>0)
                {
                    return Ok(reservation);
                }
                else
                {
                    return BadRequest(new { Message = "Booking failed" });
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { Message = "Internal server error", ErrorMessage = ex.Message });
            }
        }
        [HttpGet("calculatefare/{scheduleid}/{count}")]
        public async Task<IActionResult> CalcualteFare(int scheduleid,int count)
        {
            try
            {
                await _bookingManagementService.MakeBookingsExpiray();
                Schedule schedule = _scheduleRelatedServices.GetScheduleById(scheduleid);
                var route = _routeTerminalServices.GetRouteById(schedule.RouteId);
                var fare=_bookingServices.CalculateFare(route, count);
                return Ok(fare);
            }
            catch (Exception )
            {
                
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetBookingDetails(int bookingId)
        {
            try
            {
                
                await _bookingManagementService.MakeBookingsExpiray();
                _bookingManagementService.MakeBookingsCancelled();
                var bookingDto =await  _bookingServices.GetBookingDetails(bookingId);

                if (bookingDto != null)
                {
                    return Ok(bookingDto);
                }
                else
                {
                    return NotFound($"Booking with ID {bookingId} not found.");
                }
            }
            catch (Exception)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("user/{userId}/bookings")]
        public async Task<IActionResult> GetUserBookings(int userId)    
        {
            try
            {
                await _bookingManagementService.MakeBookingsExpiray();
               await _bookingManagementService.MakeBookingsCancelled();
                var user =await  _userServices.GetUserByUserId(userId);

                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found");
                }

                var userBookings = await  _bookingServices.GetUserBookings(userId);
                if(userBookings.Count != 0)
                {
                    return Ok(userBookings);
                }
                return NotFound();
            }
            catch (Exception )
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("user/bookings/{bookingId}/cancel")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                bool isBookingCancelled =  await _bookingServices.CancelBooking(bookingId);
                await _bookingManagementService.MakeBookingsCancelled();
                if (isBookingCancelled)
                {
                    return Ok(new { Message = "Booking cancelled successfully." });
                }
                else
                {
                    return BadRequest("Unable to cancel the booking. Please check the booking status or try again later.");
                }
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");
            }
        }
        

    }
}
