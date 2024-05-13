using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservationDataAccess.Data.Entites
{
    public class Route
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RouteName { get; set; }

        public int DepatureDestinationId{ get; set; }
        [ForeignKey(nameof(DepatureDestinationId))]
        public Destination DepartureDestination { get; set; }
        public int ArrivalDestinationId { get; set; }
        [ForeignKey(nameof(ArrivalDestinationId))]
        public Destination ArrivalDestination { get; set; }

        public decimal BaseFare { get; set; }
        public bool RouteStatus { get; set; }

        public int DepartureTerminalId { get; set; }
        [ForeignKey(nameof(DepartureTerminalId))]
        public Terminal Departureterminal { get; set; }

        public int ArrivalTerminalId { get; set; }
        [ForeignKey(nameof(ArrivalTerminalId))]
        public Terminal Arrivalterminal { get; set; }


        public ICollection<Schedule>? Schedules { get; set; }
    }
}
