using Microsoft.Extensions.Options;
using System.Text.Json;
using TicketReservation.App.Models;
using TicketReservationDataAccess.Data.Dtos;
using static System.Net.Mime.MediaTypeNames;

namespace TicketReservation.App.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        public AccountService(IOptions<ApiAddress> AccountApiAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri($"{AccountApiAddress.Value.BaseUrl}/User/") };
        }
        public async Task<LoginViewModel> LoginAsync(LoginViewModel loginModel)
        {
            try
            {
                var loginEndpoint = "Login";
                var response = await _httpClient.PostAsJsonAsync(loginEndpoint, loginModel);
                if (!response.IsSuccessStatusCode)
                {
                    new LoginViewModel();
                }
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<LoginViewModel>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
            catch (JsonException)
            {
                throw new Exception("Login failed: " );
            }
        }
        public async Task<UserModel> VerifyAccount(ResetPasswordModel ViewModel)
        {
            try
            {
                var loginEndpoint = "VerifyAccount";
                var response = await _httpClient.PostAsJsonAsync(loginEndpoint, ViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    new UserModel();
                }
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<UserModel>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }

            catch (JsonException)
            {

                throw new Exception("Verify Account failed: ");
            }
        }
        public async Task<bool> ResetPassword(string UserName, string Password)
        {
            try
            {
                var loginEndpoint = $"ResetPassword?UserName={UserName}&Password={Password}";
                var response = await _httpClient.PostAsync(loginEndpoint, null);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<bool>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException)
            {
                throw new Exception("Reset Password failed: ");
            }
        }
        public async Task<ErrorMessageModel> SignUpAsync(UserViewModel UserModel)
        {
            try
            {
                var loginEndpoint = $"CreateUser";
                var response = await _httpClient.PostAsJsonAsync(loginEndpoint, UserModel);

                var test = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                   return new ErrorMessageModel{
                        Success = true,
                        ErrorMessage="SucessFully User Created"
                   };
                }
                else
                {
                    return new ErrorMessageModel
                    {
                        Success = false,
                        ErrorMessage = test

                    };
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JsonException: {ex.Message}");
                throw new Exception("SignUp failed: ");
            }
        }
    }
}
