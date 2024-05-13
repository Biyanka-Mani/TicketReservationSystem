using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Dtos
{
    public class destinationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Destination name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only letters and spaces are allowed")]
        public string Destination_name { get; set; }
    }
}
