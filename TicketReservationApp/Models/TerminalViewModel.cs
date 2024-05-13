using System.ComponentModel.DataAnnotations;

namespace TicketReservation.App.Models
{
    public class TerminalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The TerminalName field is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "TerminalName must be between 10 and 100 characters.")]
        [RegularExpression(@"^[A-Za-z].*[A-Za-z\s]*$", ErrorMessage = "TerminalName should start and end with a letter and may contain spaces.")]
        public string TerminalName { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "TerminalLocation must be between 5 and 100 characters.")]
        [RegularExpression(@"^[A-Za-z].*[A-Za-z\s]*$", ErrorMessage = "TerminalLocation should start and end with a letter and may contain spaces.")]
        public string? Location { get; set; }
        public bool TerminalStatus { get; set; }
    }
}
