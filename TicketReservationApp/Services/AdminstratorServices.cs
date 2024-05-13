using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using TicketReservation.App.Models;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Services;

namespace TicketReservation.App.Services
{
    public class AdminstratorServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminstratorServices> _logger;
        public AdminstratorServices( ILogger<AdminstratorServices> logger, IOptions<ApiAddress> AccountApiAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri($"{AccountApiAddress.Value.BaseUrl}/Admin/") };
            _logger = logger;
        }

        public async Task<RouteViewModel> RouteById(int Id)
        {
            try
            {
                var EndPoint = $"Route/{Id}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<RouteViewModel>();
                    return apiResponseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new RouteViewModel();
        }
        public async Task<List<RouteViewModel>> GetRoutes()
        {
            try
            {
                var EndPoint = $"Routes";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<List<RouteViewModel>>();
                    return apiResponseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
                return new List<RouteViewModel>();
            }

        }
        public async Task<bool> SetFare(RouteModel routemodel)
        {
            try
            {
                var Endpoint = $"SetFare";
                using (var response = await _httpClient.PostAsJsonAsync(Endpoint, routemodel))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
                return false;
            }


        }
        public async Task<List<TerminalViewModel>> GetTerminals() 
       {
            try
            {
                var EndPoint = $"Terminals";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<TerminalViewModel>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
                return new List<TerminalViewModel>();
            }

        }
        
        public async Task<FareRequest> Routes()
        {
            try
            {
                var EndPoint = $"Routes/Routesname";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<FareRequest>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
                return new FareRequest();
            }

        }
        public async Task<List<Schedules>> GetSchedules()
        {
            try
            {
                var EndPoint = $"SchedulesView";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<Schedules>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
                return new List<Schedules>();
            }

        }
        public async Task<List<Schedules>> GetAvalaibleScheduleForRoute(int SelectedStartingPointId, int SelectedEndPointId)
        {
            try
            {
                var EndPoint = $"routes/{SelectedStartingPointId}/{SelectedEndPointId}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<Schedules>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new List<Schedules>();
        }
        public async Task<List<Bookings>> GetBookings(int Scheduleid)
        {
            try
            {
                var EndPoint = $"bookings/{Scheduleid}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<Bookings>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            }
            return new List<Bookings>();
        }
        public async Task<decimal> GetTotalRevenueForSchedule(int scheduleid)
        {
            try
            {
                var EndPoint = $"TotalRevenue/{scheduleid}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<decimal>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            } return new decimal();
        }
        public async Task<bool> CancelScheduleBookings(int scheduleid)
        {
            try
            {
                var EndPoint = $"cancelAllbookings/{scheduleid}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, scheduleid))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            }
            return false;
        }
        public async Task<bool> AddTerminal(TerminalViewModel terminalViewModel)
        {
            try
            {
                var EndPoint = $"addTerminal";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, terminalViewModel))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return false;
        }
        public async Task<bool> EditTerminal(TerminalViewModel terminalViewModel)
        {
            try
            {
                var EndPoint = $"editTerminal/{terminalViewModel.Id}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, terminalViewModel))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return false;
        }
        public async Task<bool> EditRoute(RouteViewModel routeViewModel)
        {
            try
            {
                var EndPoint = $"editRoute";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, routeViewModel))
                {

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return false;
        }
        public async Task<bool> EditSchedule(Schedules scheduleViewModel)
        {
            try
            {
                var EndPoint = $"editSchedule/{scheduleViewModel.ScheduleId}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, scheduleViewModel))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return false;
        }
        public async Task<TerminalViewModel> TermainlById(int Id)
        {
            try
            {
                var EndPoint = $"Terminal/{Id}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<TerminalViewModel>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new TerminalViewModel();
        }

        public async Task<Schedules> GetSchedulesById(int id)
        {
            try
            {
                var EndPoint = $"Schedules/{id}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<Schedules>(apiResponseString, options);
                    return apiResonse;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new Schedules();
        }
        public async Task<bool> AddRoute(RouteViewModel route)
        {
            try
            {
                var EndPoint = $"addRoute";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, route))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return false;
        }
        public async Task<bool> AddSchedule(Schedules scheduleView)
        {
            try
            {
                var EndPoint = $"addSchedule";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, scheduleView))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return false;
        }
        public async Task<List<TerminalViewModel>> GetAllAvaliableTerminals()
        {
            try
            {
                var EndPoint = $"GetAllAvaliableTerminals";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<TerminalViewModel>>(apiResponseString, options);
                    return apiResonse;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new List<TerminalViewModel>();
        }
        public async Task<ScheduleAddRequset> GetAvaliableRoutesAndVechicle()
        {
            try
            {
                var EndPoint = $"AddScheduleRequest";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<ScheduleAddRequset>(apiResponseString, options);
                    return apiResonse;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new ScheduleAddRequset();
        }
        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                var EndPoint = $"GetAllUsers";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<UserModel>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new List<UserModel>();
        }
        public async Task<bool> ScheduleDeletion(int scheduleid)
        {
            try
            {
                var EndPoint = $"ScheduleDeleteion/{scheduleid}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, scheduleid))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                return false;
                
            }
        }
        public async Task<bool> RouteDeletion(int routeid)
        {
            try
            {
                var EndPoint = $"RouteDeleteion/{routeid}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, routeid))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        public async Task<bool> TerminalDeletion(int terminalid)
        {
            try
            {
                var EndPoint = $"TerminalDeleteion/{terminalid}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, terminalid))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<bool>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        public async Task<List<UserModel>> GetAllUserBookings(int userid)
        {
            try
            {
                var EndPoint = $"bookings/{userid}";
                using (var response = await _httpClient.PostAsJsonAsync(EndPoint, userid))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<UserModel>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new List<UserModel>();

        }
        internal async  Task<bool> RemoveDestination(int id)
        {
            try
            {
                var Endpoint = $"RemoveDestination/{id}";
                using (var response = await _httpClient.PostAsJsonAsync(Endpoint, id))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<bool>();
                    return apiResponseString;
                }
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
        internal async Task<bool> AddDestination(DestinationViewModel destinationViewModel)
        {
            try
            {
                var Endpoint = $"AddDestination";
                using (var response = await _httpClient.PostAsJsonAsync(Endpoint, destinationViewModel))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<bool>();
                    return apiResponseString;
                }
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
        internal async Task<List<VechicleViewModel>> GetAllVehicles()
        {
            try
            {
                var EndPoint = $"vehicles";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var apiResponseString = await response.Content.ReadAsStringAsync();
                    var apiResonse = JsonSerializer.Deserialize<List<VechicleViewModel>>(apiResponseString, options);
                    return apiResonse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred .");
            }
            return new List<VechicleViewModel>();

        }
        internal async Task<bool> RemoveVehicle(int id)
        {
            try
            {
                var Endpoint = $"RemoveVehicle/{id}";
                using (var response = await _httpClient.PostAsJsonAsync(Endpoint, id))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<bool>();
                    return apiResponseString;
                }
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
        internal async Task<bool> AddVehicle(VechicleViewModel ViewModel)
        {
            try
            {
                var Endpoint = $"AddVehicle";
                using (var response = await _httpClient.PostAsJsonAsync(Endpoint, ViewModel))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<bool>();
                    return apiResponseString;
                }
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
    }
}
