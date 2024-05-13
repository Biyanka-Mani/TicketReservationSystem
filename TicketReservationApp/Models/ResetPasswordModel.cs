using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class ResetPasswordModel
    {
        public int Id { get; set; } 
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone number should only contain numbers.")]
        [StringLength(10)]
        public string Phone { get; set; }

        public bool IsValidated=false;
       
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters.")]
        public string? Password {  get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
     
    
    }
}
