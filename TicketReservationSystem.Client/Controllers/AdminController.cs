using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketReservationDataAccess.Data.Services;
using TicketReservationDataAccess.Data.Dtos;
using System.Runtime.CompilerServices;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        private readonly IRouteTerminalServices _routeTerminalServices;
        private readonly BookingManagementService _BookingManagementService;
        private readonly IBookingServices _bookingServices;
        private readonly IScheduleRelatedServices _scheduleRelatedServices;
        private readonly IDataMappingAndAssociationHandler Services;
        public AdminController(IAdminServices adminServices,IRouteTerminalServices routeTerminalServices, BookingManagementService BookingManagementService, IBookingServices bookingServices,IScheduleRelatedServices scheduleRelatedServices,IDataMappingAndAssociationHandler _Services)
        {
            _adminServices = adminServices;
            _routeTerminalServices = routeTerminalServices;
            _BookingManagementService = BookingManagementService;
            _bookingServices = bookingServices;
            _scheduleRelatedServices = scheduleRelatedServices;
            Services = _Services;
           
        }
        private async Task ManageBookings()
        {
            await _BookingManagementService.MakeBookingsExpiray();
            await _BookingManagementService.MakeBookingsCancelled();
        }
        [HttpGet("Route/{Id}")]
        public async Task<IActionResult> GetRoute(int Id)
        {
            try
            {
                RouteDto route = await _routeTerminalServices.GetRoute(Id);
                return Ok(route);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("addRoute")]
        public async Task<IActionResult> AddRoute(RouteDto routeDto)
        {
            try
            {
                bool addResult = await _routeTerminalServices.AddRoute(routeDto);
                if (addResult)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(false);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("editRoute")]
        public async Task<IActionResult> EditRoute(RouteDto routeDto)
        {
            try
            {
               await _adminServices.CheckRouteStatus(routeDto.id, routeDto.RouteStatus);

                bool result = await _routeTerminalServices.EditRoute(routeDto.id, routeDto);
                if (result)
                {
                    return Ok(result); 
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
        [HttpGet("Routes")]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                List<RouteDto> routes = await _routeTerminalServices.GetAllroutes();
                return Ok(routes);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("Routes/Routesname")]
        public async Task<IActionResult> Routes()
        {
            try
            {
                FareRequsetDto fareSetRequestDto = await _routeTerminalServices.Routes();
                return Ok(fareSetRequestDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("RouteDeleteion/{routeid}")]
        public async Task<IActionResult> RouteDeleteion(int routeid) 
        {
            try
            {
                await ManageBookings();
                List<ScheduleDto> scheduleDtos = Services.GetAllAssoiciatedSchedules(routeid);
                if(scheduleDtos.Count > 0)
                {
                    if (_scheduleRelatedServices.IsSchedulesScheduled(scheduleDtos))
                    {
                        return Ok(false);
                    }
                    if (await _bookingServices.GetBookingsandDelete(scheduleDtos))
                    {
                        return Ok(false);

                    }
                    if (!await _scheduleRelatedServices.DeleteSchedules(scheduleDtos))
                    {
                        return Ok(false);
                    }
                }
                if (!await _routeTerminalServices.DeleteRoute(routeid))
                {
                    return Ok(false);
                }
                return Ok(true);
            }
            catch 
            { 
                return BadRequest(); 
            }
        }



        [HttpPost("addTerminal")]
        public async Task<IActionResult> AddTerminal(TerminalDto terminalDto)
        {
            bool addResult = await _routeTerminalServices.AddTerminal(terminalDto);
            if (addResult)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to add terminal");
            }
        }
        [HttpGet("Terminal/{Id}")]
        public async Task<IActionResult>GetTerminal(int Id)
        {
            try
            {
               TerminalDto terminal=await  _routeTerminalServices.GetTerminal(Id);
                return Ok(terminal);
            }
            catch 
            { 
                return BadRequest(); 
            }
        }
        [HttpPost("editTerminal/{terminalId}")]
        public async Task<IActionResult> EditTerminal(TerminalDto terminalDto)
        {
            try
            {
                await _adminServices.CheckTerminalStatus(terminalDto.Id, terminalDto.TerminalStatus);
                await _routeTerminalServices.EditTerminal(terminalDto.Id, terminalDto);
                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }
        [HttpGet("GetAllAvaliableTerminals")]
        public async Task<IActionResult> getTerminals()
        {
            try
            {
                List<TerminalDto> terminals = await _routeTerminalServices.getallAvaliabletermainls();
                if (terminals == null)
                {
                    return NotFound();
                }
                return Ok(terminals);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("Terminals")]
        public async Task<IActionResult> GetTerminals()
        {
            try
            {
                List<TerminalDto> terminals = await _routeTerminalServices.GetAlltermainls();
                return Ok(terminals);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("TerminalDeleteion/{terminalid}")]
        public async Task<IActionResult> TerminalDeleteion(int terminalid)
        {
            try
            {
                await ManageBookings();
                List<RouteDto> routeDto = await Services.GetAllAssoiciatedRoutes(terminalid);
                if (routeDto.Count > 0)
                {
                    foreach (var route in routeDto)
                    {
                        List<ScheduleDto> scheduleDtos = Services.GetAllAssoiciatedSchedules(route.id);
                        if (scheduleDtos.Count > 0)
                        {
                            if (_scheduleRelatedServices.IsSchedulesScheduled(scheduleDtos))
                            {
                                return Ok(false);
                            }

                            if (await _bookingServices.GetBookingsandDelete(scheduleDtos))
                            {
                                return Ok(false);
                                    
                            }
                            if (!await _scheduleRelatedServices.DeleteSchedules(scheduleDtos))
                            {
                                return Ok(false);
                            }
                        }
                    }
                    if (!await _routeTerminalServices.DeleteRoutes(routeDto))
                    {
                        return Ok(false);
                    }
                }
                if (!await _routeTerminalServices.DeleteTerminal(terminalid))
                {
                    return Ok(false);
                }
                return Ok(true);
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpGet("Schedules/{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            try
            {
                await _BookingManagementService.MakeBookingsExpiray();
                ScheduleDto schedule = await _scheduleRelatedServices.GetSchedule(id);
                return Ok(schedule);
            }
            catch 
            {
                return BadRequest(); 
            }
        }
        [HttpGet("SchedulesView")]
        public async Task<IActionResult> GetSchedules()
        {
            try
            {
                List<ScheduleDto> schedules = await _scheduleRelatedServices.GetAllSchedules();
                return Ok(schedules);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("addSchedule")]
        public async Task<IActionResult> AddSchedule(ScheduleDto scheduleDto)
        {

            bool addResult = await _scheduleRelatedServices.AddSchedule(scheduleDto);

            if (addResult)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }
        [HttpPost("editSchedule/{ScheduleId}")]
        public async Task<IActionResult> EditSchedule(ScheduleDto scheduleDto)
        {
            try
            {
                await _adminServices.CheckScheduleStatus(scheduleDto.ScheduleId, scheduleDto.IsActiveSchedule);
                bool result = await _scheduleRelatedServices.EditSchedule(scheduleDto.ScheduleId, scheduleDto);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

            }
            catch (Exception)
            {
                return BadRequest(false);
            }
        }
        [HttpPost("ScheduleDeleteion/{scheduleid}")]
        public async Task<IActionResult> ScheduleDeleteion(int scheduleid)
        {
            try
            {
                await ManageBookings();
                Schedule schedule =_scheduleRelatedServices.GetScheduleById(scheduleid);
               if(schedule.ScheduleStatusEnum == TicketReservationDataAccess.Data.Entites.Models.StatusEnum.Scheduled)
               {
                    return Ok(false);
               }
                List<BookingDto> bookingDtos = Services.GetAllAssoiciatedBookings(scheduleid);
                if (_bookingServices.IsBookingsBooked(bookingDtos))
                {
                    return Ok(false);
                }
                if (!await _bookingServices.DeleteBookings(bookingDtos))
                {
                    return Ok(false);
                }
                if (!await _scheduleRelatedServices.DeleteSchedule(scheduleid))
                {
                    return Ok(false);
                }
                return Ok(true);
            }
            catch 
            {
                return BadRequest(); 
            }
        }
        [HttpGet("AddScheduleRequest")]
        public async Task<IActionResult> ScheduleAddRequset()
        {
            try
            {
                ScheduleAddRequsetDto scheduleAdd = await _scheduleRelatedServices.ScheduleAddRequset();
                if (scheduleAdd == null)
                {
                    return NotFound();
                }
                return Ok(scheduleAdd);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("SetFare")]
        public async Task<IActionResult> SetFare(RouteModelDto routeModelDto)
        {
            try
            {
                bool FareSet = await _adminServices.SetFare(routeModelDto.RouteId, routeModelDto.fare); ;
                if (FareSet)
                {
                    return Ok(true);
                }
                return BadRequest();
                
            }
            catch (Exception )
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("cancelAllbookings/{scheduleid}")]
        public async Task<IActionResult>CancelAllBookingsSchedule(int scheduleid)
        {
            try
            {
                await _BookingManagementService.MakeBookingsExpiray();
                bool Iscancelled=await _bookingServices.CancelBookingWithScheduleId(scheduleid);
                await _BookingManagementService.MakeBookingsCancelled();
                if (Iscancelled)
                {
                    return Ok(true);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<UserIdDto> users=await _adminServices.GetAllUsers();
                return Ok(users);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("routes/{SelectedStartingPointId}/{SelectedEndPointId}")]
        public async Task<IActionResult> GetRoutesByLocation(int SelectedStartingPointId,int SelectedEndPointId)
        {
            try
            {
                await _BookingManagementService.MakeBookingsExpiray();

                int routeId = await _routeTerminalServices.GetRouteIdByLocation(SelectedStartingPointId, SelectedEndPointId);
                if (routeId == 0) 
                { 
                    return Ok("No routes found for the specified locations."); 
                }
                var schedules = _scheduleRelatedServices.GetSchedulesForRoute(routeId);
               
                return Ok(schedules);

            }
            catch (Exception )
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("bookings/{Scheduleid}")]
        public async Task<IActionResult> GetBookingsWithScheduleId(int scheduleid)
        {
            try
            {
                await ManageBookings();
                var bookings = _bookingServices.GetBookingswithScheduleId(scheduleid);
                if(bookings == null) 
                { 
                    return NotFound(); 
                }
                return Ok(bookings);
            }
            catch (Exception )
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("TotalRevenue/{scheduleid}")]
        public async Task<IActionResult> GetTotalRevenueFromSchedule(int scheduleid)
        {
            try
            {
                decimal Revenue = _adminServices.GetTotalRevenueFromSchedule(scheduleid);
                if (Revenue == null)
                { 
                    return NotFound(); 
                }
                return Ok(Revenue);
            }
            catch (Exception )
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("bookings/{userid}")]
        public async Task<IActionResult> GetUserBookings( int userid)
        {
            try
            {
                await ManageBookings();
                var userBookings =  _bookingServices.GetUserBookings(userid);
                var userIdDto = new UserIdDto
                {
                    Id = userid,
                    Bookings =await userBookings
                };

                return Ok(new List<UserIdDto> { userIdDto });
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("destinations")]
        public async Task<IActionResult> GetDestinations()
        {
            try
            {
                List<destinationDto> destinations = await _routeTerminalServices.GetAllDestinations();
                return Ok(destinations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("RemoveDestination/{id}")]
        public async Task<IActionResult> RemoveDestination(int id)
        {
            try
            {
               List<RouteDto>? routes= Services.GetAllAssoiciatedRoute(id);

                foreach(var route in routes)
                {
                    List<ScheduleDto> schedule=Services.GetAllAssoiciatedSchedules(route.id);
                    foreach (var scheduleDto in schedule)
                    {
                        if(scheduleDto.IsActiveSchedule)
                        {
                            return Ok(false);
                        }
                    }
                }
                bool IsdestinationDeleted = await _routeTerminalServices.RemoveDestination(id);
                return Ok(IsdestinationDeleted);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("AddDestination")]
        public async Task<IActionResult> AddDestination(destinationDto destinationDto)
        {
            try
            {
                bool IsdestinationAddded = await _routeTerminalServices.AddDestination(destinationDto);
                return Ok(IsdestinationAddded);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("vehicles")]
        public async Task<IActionResult> GetVehicles()
        {
            try
            {
                List<VehicleDto> vehicles = await _routeTerminalServices.GetAllVehicles();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("RemoveVehicle/{id}")]
        public async Task<IActionResult> RemoveVehicle(int id)
        {
            try
            {
                List<ScheduleDto>? schedule = Services.GetAllAssoiciatedSchedulesForVehicle(id);
                if(schedule!=null)
                {
                    foreach (var scheduleDto in schedule)
                    {
                        if (scheduleDto.IsActiveSchedule)
                        {
                            return Ok(false);
                        }
                    }
                }

                bool IsvehicleDeleted = await _routeTerminalServices.RemoveVehicle(id);
                return Ok(IsvehicleDeleted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle(VehicleDto vehicleDto)
        {
            try
            {
                bool IsvehicleAddded = await _routeTerminalServices.AddVehicle(vehicleDto);
                return Ok(IsvehicleAddded);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("maxBookingsRoutes")]
        public async Task<IActionResult> routesWithMaxBookings()
        {
            try
            {
                List<RouteModelDto> routes = new List<RouteModelDto>();
                HashSet<int> processedRoutes = new HashSet<int>();

                List<int> scheduleIDs = _bookingServices.GetMaxBookedScheduleIds();
                foreach (int scheduleID in scheduleIDs)
                {
                    // Check if the schedule -processed
                    if (!processedRoutes.Contains(scheduleID))
                    {
                        List<int> routeIds = _routeTerminalServices.GetUniqueRoutesForSchedule(scheduleID);
                        foreach (int routeId in routeIds)
                        {
                            // Check if the route - processed
                            if (!processedRoutes.Contains(routeId))
                            {
                                List<ScheduleDto> schedules = Services.GetAllAssoiciatedSchedules(routeId);
                                decimal revenuefromParticularSchedule = 0;
                                foreach (ScheduleDto schedule in schedules)
                                {
                                    revenuefromParticularSchedule += _adminServices.GetTotalRevenueFromSchedule(schedule.ScheduleId);
                                }
                                RouteModelDto model = _routeTerminalServices.RouteModel(routeId);
                                model.TotalRevenue = revenuefromParticularSchedule;
                                routes.Add(model);
                                // Mark the route- processed
                                processedRoutes.Add(routeId);
                            }
                        }
                        // Mark the schedule- processed
                        processedRoutes.Add(scheduleID);
                    }
                }
                return Ok(routes);
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
