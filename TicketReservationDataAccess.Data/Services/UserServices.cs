using Microsoft.EntityFrameworkCore;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public class UserServices : IUserServices
    {
        private readonly TicketReservationDBContext _reservationDBContext;

        public UserServices(TicketReservationDBContext reservationDBContext)
        {
            _reservationDBContext = reservationDBContext;
        }

        public async Task<AppUser> CreateAccount(UserDto user)
        {

            AppUser appUser = new AppUser()
            {
                Username = user.Username,
                Userpassword = ValidationServices.HashPassword(user.Userpassword),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Age = user.Age,
                UserRole = true,
                IdentityCardNumber = user.IdentityCardNumber,
                BloodGroup = user.BloodGroup,
                SeatPreference = user.SeatPreference,

            };
            _reservationDBContext.AppUsers.Add(appUser);
            await _reservationDBContext.SaveChangesAsync();
            return appUser;
        }
        public async Task<UserDto> GetProfile(int userid)
        {
            AppUser user = await _reservationDBContext.AppUsers.FirstOrDefaultAsync(user => user.Id == userid);
            UserDto userdto = MapUserEntityToDto(user);
            return userdto;

        }
        public bool ChangePassword(AppUser user, string newpassword)
        {
            user.Userpassword = ValidationServices.HashPassword(newpassword);
            _reservationDBContext.SaveChangesAsync();
            return true;
        }
        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await  _reservationDBContext.AppUsers.FirstOrDefaultAsync(user => user.Username == username);
        }
        public async Task<AppUser> GetUserByUserId(int id)
        {
            return await _reservationDBContext.AppUsers.FirstOrDefaultAsync(user => user.Id == id);
        }
        public async Task<bool> UpdateUserProfileAsync(int userId, EditUserDto userProfileUpdateDto)
        {
            var user = await _reservationDBContext.AppUsers.FindAsync(userId);

            if (user == null)
            {
                return false;
            }
            if (UserProfileUnchanged(user, userProfileUpdateDto))
            {
                return true;
            }
            user.FirstName = userProfileUpdateDto.FirstName;
            user.LastName = userProfileUpdateDto.LastName;
            user.Email = userProfileUpdateDto.Email;
            user.Phone = userProfileUpdateDto.Phone;
            user.Age = userProfileUpdateDto.Age;
            user.SeatPreference = userProfileUpdateDto.SeatPreference;

            user.BloodGroup = userProfileUpdateDto.BloodGroup;
            user.IdentityCardNumber = userProfileUpdateDto.IdentityCardNumber;

            try
            {
                await _reservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool UserProfileUnchanged(AppUser user, EditUserDto userProfileUpdateDto)
        {
            return user.FirstName == userProfileUpdateDto.FirstName
            && user.LastName == userProfileUpdateDto.LastName
            && user.Email == userProfileUpdateDto.Email
            && user.Phone == userProfileUpdateDto.Phone
            && user.Age == userProfileUpdateDto.Age
            && user.SeatPreference == userProfileUpdateDto.SeatPreference
            && user.BloodGroup == userProfileUpdateDto.BloodGroup
            && user.IdentityCardNumber == userProfileUpdateDto.IdentityCardNumber;

        }
        public UserDto MapUserEntityToDto(AppUser appUser)
        {
            return new UserDto()
            {
                Id = appUser.Id,
                Username = appUser.Username,
                Userpassword = appUser.Userpassword,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                Phone = appUser.Phone,
                Age = appUser.Age,
                BloodGroup = appUser.BloodGroup,
                IdentityCardNumber = appUser.IdentityCardNumber,
                SeatPreference = appUser.SeatPreference,
            };
        }
        

    }
}








