using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IValidationServices
    {
        Task<LoginDto> AuthenticateUser(LoginDto loginDto);
        bool DoesUserExist(string phoneNumber);
        Task<bool> IsUsernameUnique(string username);
        bool IsValidPassword(string password);
        bool IsValidPhonenumber(string phone);
        bool IsValidUsername(string username);
        Task<UserIdDto> VerifyAccount(ResetPasswordDto resetpassswordDto);
        bool VerifyPassword(AppUser user, string password);
    }
}