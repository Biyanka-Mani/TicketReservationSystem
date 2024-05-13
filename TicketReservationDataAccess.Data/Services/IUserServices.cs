using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IUserServices
    {
        bool ChangePassword(AppUser user, string newpassword);
        Task<AppUser> CreateAccount(UserDto user);
        Task<UserDto> GetProfile(int userid);
        Task<AppUser> GetUserByUserId(int id);
        Task<AppUser> GetUserByUsername(string username);
        UserDto MapUserEntityToDto(AppUser appUser);
        Task<bool> UpdateUserProfileAsync(int userId, EditUserDto userProfileUpdateDto);
    }
}