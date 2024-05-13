using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Dtos
{
    public  class FareRequsetDto
    {
        public List<RouteModelDto> routes { get; set; }

        [Required(ErrorMessage = "Please enter the fare.")]
        [Range(100, double.MaxValue, ErrorMessage = "Fare must be a non-negative value.")]

        public decimal Fare { get; set; }
    }
}
