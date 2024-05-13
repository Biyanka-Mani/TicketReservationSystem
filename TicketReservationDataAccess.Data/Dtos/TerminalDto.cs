using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Dtos
{
    public class TerminalDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The TerminalName field is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "TerminalName must be between 3 and 100 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Invalid characters in TerminalName.")]
        public string TerminalName { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "TerminalLocation must be between 3 and 100 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Invalid characters in TerminalLocation.")]
        public string? Location { get; set; }
        public bool TerminalStatus { get; set; }

    }
}
