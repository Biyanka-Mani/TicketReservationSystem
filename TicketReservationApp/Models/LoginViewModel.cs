using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public bool UserRole {  get; set; }
    }
}
