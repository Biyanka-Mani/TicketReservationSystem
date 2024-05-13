using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Dtos
{
    public  class RouteModelDto
    {

        public int RouteId { get; set; }
        public string? RouteName { get; set; }
        public decimal fare {  get; set; }
        public decimal? TotalRevenue { get; set; }
    }
}
