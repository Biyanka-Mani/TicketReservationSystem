using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketReservation.App.Models;
using TicketReservation.App.Services;
using TicketReservationDataAccess.Data.Entites;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TicketReservation.App.Controllers
{
    public class AdminstratorController : Controller
    {
        private readonly AdminstratorServices adminstratorServices;
        private readonly CommonServices commonServices;

        public AdminstratorController(AdminstratorServices _adminstratorServices,CommonServices services)
        {
            this.adminstratorServices = _adminstratorServices;
            this.commonServices = services;
        }

        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> Index()
        {
            List<RouteModel> routes = await commonServices.RoutesWithMaxBookings();
            return View(routes);
        }

        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> UserFareSearchView()
        {
            var users = await adminstratorServices.GetAllUsers();
            if (users.Count != 0)
            {
                return View(users);
            }

            TempData["ErrorMessage"] = "Unable to fetch users";
            return RedirectToAction("Index");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> SearchUserFare(int selectedUserId, string userSearch)
        {
            if (selectedUserId == 0)
            {
                TempData["ErrorMessage"] = "No User With Given Username";
                return RedirectToAction("UserFareSearchView", "Adminstrator");
            }

            var userbookings = await adminstratorServices.GetAllUserBookings(selectedUserId);
            ViewBag.SelectedUserName = userSearch;
            foreach(UserModel userModel in userbookings)
            {
                if(userModel.Bookings.Count > 0)
                {
                    return View("UserFareSearchView", userbookings);
                }
            }
            
            TempData["SuccessMessage"] = "User Have No Bookings";
            return RedirectToAction("UserFareSearchView", "Adminstrator");
        }


        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> SetFareView()
        {
            FareRequest FareRequest = await adminstratorServices.Routes();
            return View(FareRequest);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> SetFare([FromForm] RouteModel request)
        {

            bool IsSet = await adminstratorServices.SetFare(request);

            if (IsSet)
            {
                TempData["SuccessMessage"] = "Fare set successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to set fare.";
            }
            return RedirectToAction("SetFareView");
        }


        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> ViewBookingsByRouteSearch()
        {
            List<DestinationViewModel> destinations = await commonServices.GetAllDestinations();
            return View(destinations);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> ViewBookings(int SelectedStartingPointId ,int SelectedEndPointId )
        {
            var schedules = await adminstratorServices.GetAvalaibleScheduleForRoute(SelectedStartingPointId, SelectedEndPointId);
            if (schedules.Count != 0)
            {
                return View("Schedules", schedules);
            }
            TempData["ErrorMessage"] = "Schedules Not Avaliable!";
            return RedirectToAction("ViewBookingsByRouteSearch", "Adminstrator");

        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> SchedulesViewBooking()
        {
            List<Schedules> scheduleViewModel = await adminstratorServices.GetSchedules();
            if (scheduleViewModel != null)
            {
                return View(scheduleViewModel);
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("ViewSchedules", "Adminstrator");
        }



        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int Bookingid,int scheduleId)
        {
            if (await commonServices.CancelBooking(Bookingid))
            {
                TempData["SuccessMessage"] = "Booking Cancelled!";
                return RedirectToAction("Bookings", new { scheduleId });
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("Bookings", new { scheduleId });
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> Bookings(int scheduleId)
        {
            var bookings=await adminstratorServices.GetBookings(scheduleId);
            if(bookings.Count != 0)
            {
                return View("Bookings", bookings);
            }
            return View("NoBookings");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> RevenueForSchedule(int scheduleid)
        {
            var TotalRevenue=await adminstratorServices.GetTotalRevenueForSchedule(scheduleid);
            ViewBag.TotalRevenue = TotalRevenue;
            return View();
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> CancelAllBookings(int scheduleid)
        {
            bool cancelled=await adminstratorServices.CancelScheduleBookings(scheduleid);
            if (cancelled)
            {
                TempData["SuccessMessage"] = "All Schedule Bookings Cancelled!";
            }
            else
            {
                TempData["ErrorMessage"] = " Schedule Bookings Not Cancelled ";
            }
            var bookings = await adminstratorServices.GetBookings(scheduleid);
            if (bookings.Count != 0)
            {
                return View("Bookings", bookings);
            }
            return View("NoBookings");

        }


        //Terminal

        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> AddTerminalView()
        {
            var model = new TerminalViewModel();
            return View(model);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> EditTerminalView(int terminalid)
        {   
            var terminal=await adminstratorServices.TermainlById(terminalid);
            return View("AddTerminalView",terminal);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> EditTerminal(TerminalViewModel terminalViewModel)
        {
            bool terminalUpdated=await adminstratorServices.EditTerminal(terminalViewModel);
            if(terminalUpdated)
            {
                TempData["SuccessMessage"] = "Terminal Updated successfully!";
                return RedirectToAction("Terminals", "Adminstrator");

            }
            TempData["ErrorMessage"] = "Terminal Not Updated";
            return RedirectToAction("Terminals", "Adminstrator");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> AddTerminal(TerminalViewModel terminalViewModel)
        {
            bool terminalAdded = await adminstratorServices.AddTerminal(terminalViewModel);
            if(terminalAdded)
            {
                TempData["SuccessMessage"] = "Terminal Added successfully!";
                return RedirectToAction("Terminals","Adminstrator");

            }
            TempData["ErrorMessage"] = "Terminal Not Added";
            return RedirectToAction("Terminals", "Adminstrator");

        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> Terminals()
        {
            List<TerminalViewModel> terminalViewModel = await adminstratorServices.GetTerminals();
            if (terminalViewModel != null)
            {
                return View(terminalViewModel);
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("Terminals", "Adminstrator");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> TerminalDeleteion(int TerminalId)
        {
            bool TerminalDeleted = await adminstratorServices.TerminalDeletion(TerminalId);
            if (TerminalDeleted)
            {
                TempData["SuccessMessage"] = "Terminal Deleted Successfully!";
                return RedirectToAction("Terminals", "Adminstrator");

            }
            TempData["ErrorMessage"] = "Terminal Cannot be Deleted";
            return RedirectToAction("Terminals", "Adminstrator");

        }

        //Route


        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> Routes()
        {
            List<RouteViewModel> routeViewModel = await adminstratorServices.GetRoutes();
            if (routeViewModel != null)
            {
                return View(routeViewModel);
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("Routes", "Adminstrator");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> AddRouteView()
        {
            var model = new AddeditRouteModel
            {
                Route = new RouteViewModel(),
                destinations = await commonServices.GetAllDestinations(),
                Terminals = await adminstratorServices.GetAllAvaliableTerminals()
            };

            return View(model);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> EditRouteView(int routeId)
        {
            var route = await adminstratorServices.RouteById(routeId);
            var model = new AddeditRouteModel
            {
                Id = routeId,
                destinations = await commonServices.GetAllDestinations(),
                Route = route,
                Terminals = await adminstratorServices.GetAllAvaliableTerminals()
            };

            return View("AddRouteView", model);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> EditRoute(RouteViewModel routeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during Editing Route");
                var route = await adminstratorServices.RouteById(routeViewModel.id);
                var model = new AddeditRouteModel
                {
                    Id = routeViewModel.id,
                    destinations = await commonServices.GetAllDestinations(),
                    Route = route,
                    Terminals = await adminstratorServices.GetAllAvaliableTerminals()
                };
                return View("AddRouteView", model);
            }
            bool terminalUpdated = await adminstratorServices.EditRoute(routeViewModel);
            if (terminalUpdated)
            {
                TempData["SuccessMessage"] = "Route Updated successfully!";
                return RedirectToAction("Routes","Adminstrator");

            }
            TempData["ErrorMessage"] = "Route Not Updated";
            return RedirectToAction("Routes", "Adminstrator");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> AddRoute(RouteViewModel routeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during Adding Route");
                TempData["ErrorMessage"] = "Please Check Entered values!";
                var model = new AddeditRouteModel
                {
                    Route = routeViewModel,
                    destinations = await commonServices.GetAllDestinations(),
                    Terminals = await adminstratorServices.GetAllAvaliableTerminals()
                };
                return View("AddRouteView", model);
            }
            bool routeAdded = await adminstratorServices.AddRoute(routeViewModel);
            if (routeAdded)
            {
                TempData["SuccessMessage"] = "Route added successfully!";
                return RedirectToAction("Routes", "Adminstrator");
            }

            TempData["ErrorMessage"] = "Route not added";
            return RedirectToAction("Routes", "Adminstrator");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> RouteDeleteion(int RouteId)
        {
            bool RouteDeleted = await adminstratorServices.RouteDeletion(RouteId);
            if (RouteDeleted)
            {
                TempData["SuccessMessage"] = "Route Deleted Successfully!";
                return RedirectToAction("Routes", "Adminstrator");

            }
            TempData["ErrorMessage"] = "Route Cannot be Deleted!";
            return RedirectToAction("Routes", "Adminstrator");

        }

        //schedule

        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> ViewSchedules()
        {
            List<Schedules> scheduleViewModel = await adminstratorServices.GetSchedules();
            if (scheduleViewModel != null)
            {
                return View(scheduleViewModel);
            }
            TempData["ErrorMessage"] = "Something Went wrong!";
            return RedirectToAction("ViewSchedules", "Adminstrator");

        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> AddScheduleView()
        {
            var model = new AddEditScheduleModel
            {
                scheduleAddRequset = await adminstratorServices.GetAvaliableRoutesAndVechicle(),
                Schedule = new Schedules()
            };
            
            return View(model);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> EditScheduleView(int scheduleId)
        {
            var schedule = await adminstratorServices.GetSchedulesById(scheduleId);
            var model = new AddEditScheduleModel
            {
                Id = scheduleId,
                // Populate other properties of RouteViewModel from the 'route' object
                Schedule=schedule,
                scheduleAddRequset = await adminstratorServices.GetAvaliableRoutesAndVechicle(),
            };

            return View("AddScheduleView", model);
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> EditSchedule(Schedules schedules)
        {
            bool terminalUpdated = await adminstratorServices.EditSchedule(schedules);
            if (terminalUpdated)
            {
                TempData["SuccessMessage"] = "Schedule Updated successfully!";
                return RedirectToAction("ViewSchedules", "Adminstrator");

            }
            TempData["ErrorMessage"] = "Schedule Not Updated";
            return RedirectToAction("ViewSchedules", "Adminstrator");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> AddSchedule(Schedules scheduleViewModel)
        {
            if(scheduleViewModel.DepartureTime > scheduleViewModel.ArrivalTime)
            {
                TempData["ErrorMessage"] = "Unsuccessfull Schedule Addition Enter Valid Date";
                return RedirectToAction("AddScheduleView", "Adminstrator");
            }
            bool scheduleAdded = await adminstratorServices.AddSchedule(scheduleViewModel);
            if (scheduleAdded)
            {
                TempData["SuccessMessage"] = "Schedule Added successfully!";
                return RedirectToAction("ViewSchedules", "Adminstrator");

            }
            TempData["ErrorMessage"] = "Schedule Not Added";
            return RedirectToAction("ViewSchedules", "Adminstrator");

        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> ScheduleDeleteion(int scheduleId)
        {
            bool ScheduleDeleted = await adminstratorServices.ScheduleDeletion(scheduleId);
            if (ScheduleDeleted)
            {
                TempData["SuccessMessage"] = "Schedule Deleted Successfully!";
                return RedirectToAction("ViewSchedules", "Adminstrator");

            }
            TempData["ErrorMessage"] = "Schedule Cannot be Deleted!";
            return RedirectToAction("ViewSchedules", "Adminstrator");

        }



        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> DestinationView()
        {
            List<DestinationViewModel> destinations = await commonServices.GetAllDestinations();
            return View(destinations);
           
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            bool DestinationDeleted=await adminstratorServices.RemoveDestination(id);
            if (DestinationDeleted)
            {
                TempData["SuccessMessage"] = "Destination removed successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Destination not Removed";
            }
            return RedirectToAction("DestinationView");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> AddDestination(DestinationViewModel destinationViewModel)
        {
            if (ModelState.IsValid)
            {
                bool IsDestinationAdded = await adminstratorServices.AddDestination(destinationViewModel);
                if (IsDestinationAdded)
                {
                    TempData["SuccessMessage"] = "Destination added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Destination not added";
                }
                return RedirectToAction("DestinationView");
            }
            TempData["ErrorMessage"] = "Destination name is required,Only letters and spaces are allowed ";
            return RedirectToAction("DestinationView");

        }



        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpGet]
        public async Task<IActionResult> VehiclesView()
        {
            List<VechicleViewModel> vehicles = await adminstratorServices.GetAllVehicles();
            return View(vehicles);

        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            bool VehicleDeleted = await adminstratorServices.RemoveVehicle(id);
            if (VehicleDeleted)
            {
                TempData["SuccessMessage"] = "Vehicle removed successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Vehicle Cannot  Removed";
            }
            return RedirectToAction("VehiclesView");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        public async Task<IActionResult> AddVechicleView()
        {
            return View("AddVehicleView");
        }
        [Authorize(AuthenticationSchemes = "AdminCookies")]
        [HttpPost]
        public async Task<IActionResult> AddVehicle(VechicleViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                bool IsVehicleAdded = await adminstratorServices.AddVehicle(ViewModel);
                if (IsVehicleAdded)
                {
                    TempData["SuccessMessage"] = "Vehicle added successfully!";
                    return RedirectToAction("VehiclesView");
                }
                else
                {
                    TempData["ErrorMessage"] = "Vehicle not added";
                    return View("AddVehicleView", ViewModel);
                }
            }
            return View("AddVehicleView", ViewModel);

        }
    }
}
