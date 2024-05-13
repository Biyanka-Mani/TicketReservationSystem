using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Dtos
{
   public class ScheduleAddRequsetDto
   {
        public IEnumerable<RouteDto> Route { get; set;}
        public IEnumerable<VehicleDto> Vechicle {  get; set;}

   }
}
