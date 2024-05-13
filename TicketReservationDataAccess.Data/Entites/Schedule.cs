using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Entites.Models;

namespace TicketReservationDataAccess.Data.Entites
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RouteId { get; set; }
        [ForeignKey(nameof(RouteId))]
        public Route Route { get; set; }

        public int VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public ModeOfTransport ModeOfTransport { get; set; }
        public int AvaliablityOfSeats { get; set; }
        public StatusEnum ScheduleStatusEnum { get; set; }
        public TimeOfDayEnum TimeOfDay { get; set; }

        public bool IsActiveSchedule { get; set; }

        public ICollection<Booking>? Bookings { get; set;}

    }
}
