using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TicketReservation.App.Models;
using TicketReservation.App.Models.Enums;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;
using TimeOfDayEnum = TicketReservation.App.Models.Enums.TimeOfDayEnum;

namespace TicketReservation.App.Services
{
    public class CustomerServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerServices> _logger;
        public CustomerServices(ILogger<CustomerServices> logger, IOptions<ApiAddress> AccountApiAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri($"{AccountApiAddress.Value.BaseUrl}/User/") };
            _logger = logger;
        }
        public async Task<List<Bookings>> GetAllUserBookings(int id)
        {
            try
            {
                var Endpoint = $"user/{id}/bookings";
                using (var response = await _httpClient.GetAsync(Endpoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<List<Bookings>>();
                    return apiResponseString;
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while fetching user bookings.");
            }
            return new List<Bookings>();

        }
        public async Task<DetailsBooking> GetDetailsOfBooking(int id)
        {
            try
            {
                var Endpoint = $"booking/{id}";
                using (var response = await _httpClient.GetAsync(Endpoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<DetailsBooking>();
                    return apiResponseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            }
            return new DetailsBooking();
        }
        public async Task<List<Schedules>> SeeSchedules(int SelectedStartingPointId,int SelectedEndPointId)
        {
            try
            {
                var Endpoint = $"SeeSchedules/{SelectedStartingPointId}/{SelectedEndPointId}";
                using (var response = await _httpClient.GetAsync(Endpoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<List<Schedules>>();
                    return apiResponseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            }
            return new List<Schedules>();
        }
        public async Task<List<Schedules>> FilterSchedules(TimeOfDayEnum timeOfDay, Models.Enums.ModeOfTransport modeOfTransport,int SelectedStartingPointId, int SelectedEndPointId, int NumberOfPassengers)
        {
            try
            {
                List<Schedules> schedules =await GetAvalaibleScheduleForRoute(SelectedStartingPointId, SelectedEndPointId, NumberOfPassengers);
                if (timeOfDay != 0)
                {
                    schedules = schedules.Where(schedule => schedule.TimeOfDay == timeOfDay).ToList();
                }

                if (modeOfTransport != 0)
                {
                    schedules = schedules.Where(schedule => schedule.modeOfTransport == modeOfTransport).ToList();
                }

                return schedules;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            }
            return new List<Schedules>();
        }
        public async Task<List<Schedules>> GetAvalaibleScheduleForRoute(int SelectedStartingPointId,int SelectedEndPointId,int NumberOfPassengers)
        {
            try
            {
                var EndPoint = $"routes/{SelectedStartingPointId}/{SelectedEndPointId}/{NumberOfPassengers}";
                using (var response = await _httpClient.GetAsync(EndPoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<List<Schedules>>();
                    return apiResponseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user booking.");
            }
            return new List<Schedules>();
        }
        public async Task<Bookings>MakeReservation(BookingRequest bookingRequest)
        {
            try
            {
                var Endpoint = $"ReserveTickets/{bookingRequest.scheduleid}";
                using(var response = await _httpClient.PostAsJsonAsync(Endpoint, bookingRequest))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<Bookings>();
                    return apiResponseString;
                }
                
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
        public async Task<decimal> FareCalculation(int scheduleid,int count)
        {
            try
            {
                var Endpoint = $"calculatefare/{scheduleid}/{count}";
                using (var response = await _httpClient.GetAsync(Endpoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<decimal>();
                    return apiResponseString;
                }

            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
        public async Task<bool> ChangePasswordAsync(int id,ChangePasswordViewModel model)
        {
            try
            {
                var Endpoint = $"ChangePassword/{id}";
                using (var response = await _httpClient.PostAsJsonAsync(Endpoint, model))
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
        public async Task<UserViewModel>GetProfile(int userid)
        {

            try
            {
                var Endpoint = $"user/EditProfile/{userid}";
                var response = await _httpClient.GetAsync(Endpoint);
                var apiResponseString = await response.Content.ReadFromJsonAsync<UserViewModel>();
                return apiResponseString;
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
        public async Task<bool> EditProfile(EditUserViewModel userViewModel)
        {
            try
            {
                var Endpoint = $"profileEdit";
                var response = await _httpClient.PostAsJsonAsync(Endpoint, userViewModel);
                var apiResponseString = await response.Content.ReadFromJsonAsync<bool>();
                return apiResponseString;
            }
            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }
        }
  
    }
}
