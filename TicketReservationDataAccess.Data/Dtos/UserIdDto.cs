using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Dtos
{
    public  class UserIdDto
    {
        public int? Id { get; set; }
        public string? Username { get; set; }

        public List<BookingDto>? Bookings { get; set; }

    }
}
