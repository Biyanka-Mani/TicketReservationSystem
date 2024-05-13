using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;


namespace TicketReservationDataAccess.Data.Services
{
    public class ValidationServices : IValidationServices
    {
        private readonly TicketReservationDBContext _reservationDBContext;
        public ValidationServices(TicketReservationDBContext ticketReservationDBContext)
        {

            _reservationDBContext = ticketReservationDBContext;
        }

        public bool IsValidUsername(string username)
        {

            return string.IsNullOrEmpty(username) && username.Length >= 3;
        }
        public bool IsValidPassword(string password)
        {

            return string.IsNullOrEmpty(password) && password.Length >= 6;
        }
        public async Task<bool> IsUsernameUnique(string username)
        {
            var existingUsernames = await GetAllUsernames();

            bool isUnique = existingUsernames.Contains(username, StringComparer.OrdinalIgnoreCase);

            return !isUnique;
        }
        private async Task<IEnumerable<string>> GetAllUsernames()
        {
            var usernames = await _reservationDBContext.AppUsers.Select(user => user.Username).ToListAsync();
            return usernames;
        }
        public bool IsValidPhonenumber(string phone)
        {
            return string.IsNullOrEmpty(phone) && phone.Length >= 10;
        }
        public bool DoesUserExist(string phoneNumber)
        {
            var existingUser =  _reservationDBContext.AppUsers
                .FirstOrDefault(user => user.Phone.Equals(phoneNumber));

            if (existingUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        //login
        public async Task<LoginDto> AuthenticateUser(LoginDto loginDto)
        {
            AppUser appUser = await GetUserByUsername(loginDto.Username);

            if (appUser != null && VerifyPassword(appUser, loginDto.Password))
            {
                loginDto.Id = appUser.Id;
                if (appUser.UserRole)
                {
                    //user

                    loginDto.UserRole = true;
                    return loginDto;
                }
                loginDto.Id = appUser.Id;
                loginDto.UserRole = false;
                return loginDto;
            }
            return loginDto = null;
        }
        public async Task<UserIdDto> VerifyAccount(ResetPasswordDto resetpassswordDto)
        {
            AppUser user = await _reservationDBContext.AppUsers.FirstOrDefaultAsync(user => user.Username == resetpassswordDto.UserName);
            if (user == null)
            {
                return new UserIdDto();
            }
            if (user.Phone != resetpassswordDto.Phone || user.Email != resetpassswordDto.Email)
            {
                return new UserIdDto();

            }
            return new UserIdDto
            {
                Id = user.Id,
                Username = user.Username,
            };


        }
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }
        private async Task<AppUser> GetUserByUsername(string Username)
        {
            return await  _reservationDBContext.AppUsers.FirstOrDefaultAsync(user => user.Username == Username);
        }
        public bool VerifyPassword(AppUser user, string password)
        {
            string PasswordEntered = HashPassword(password);
            return user.Userpassword == PasswordEntered;
        }

    }
}
