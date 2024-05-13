using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Dtos
{
    public class BookRequestDto
    {
        public int scheduleid {  get; set; }
        public int NoofTickets {  get; set; }
        public int UserId { get; set; }
    }
}
