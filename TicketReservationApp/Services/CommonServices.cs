using Microsoft.Extensions.Options;
using System.Text.Json;
using TicketReservation.App.Models;

namespace TicketReservation.App.Services
{
    public class CommonServices
    {
        private readonly HttpClient _httpClient;
        public CommonServices(IOptions<ApiAddress> AccountApiAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri($"{AccountApiAddress.Value.BaseUrl}/") };
        }
        public async Task<List<RouteModel>> RoutesWithMaxBookings()
        {
            try
            {
                var Endpoint = $"Admin/maxBookingsRoutes";
                using (var response = await _httpClient.GetAsync(Endpoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<List<RouteModel>>();
                    return apiResponseString;
                }
            }
            catch (Exception ex)
            {
                return new List<RouteModel>();

            }

        }
        public async Task<List<DestinationViewModel>> GetAllDestinations()
        {
            try
            {
                var Endpoint = $"User/GetAllDestinations";
                using (var response = await _httpClient.GetAsync(Endpoint))
                {
                    var apiResponseString = await response.Content.ReadFromJsonAsync<List<DestinationViewModel>>();
                    return apiResponseString;
                }
            }

            catch (Exception)
            {
                return new List<DestinationViewModel>();
                throw new Exception("Internal server error ");
            }
        }
        public async Task<bool> CancelBooking(int Bookingid)
        {
            try
            {
                var Endpoint = $"User/user/bookings/{Bookingid}/cancel";
                var response = await _httpClient.PostAsJsonAsync(Endpoint, Bookingid);

                var test = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                return true;
            }


            catch (JsonException)
            {
                throw new Exception("Internal server error ");
            }


        }
    }
}
