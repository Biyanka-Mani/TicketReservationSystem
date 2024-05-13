using Microsoft.EntityFrameworkCore.Migrations;
using TicketReservationDataAccess.Data.Entites.Models;

#nullable disable

namespace TicketReservationDataAccess.Data.Migrations
{
    public partial class ValuesToEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
         table: "Vehicles",
         columns: new[] { "VehicleName", "Description", "Capacity" },
         values: new object[,]
         {
                { "TL 176", " Intercity Train", 280 },
                { "BS 28"   , "Double-Decker Bus", 50 },
                { "FL 549", "Domestic Flight", 300 },
                { "TL 136", "Subway/Metro Train", 200},
                { "BS 94", "Coach Bus", 30 },
                { "FL 596", "Domestic Flight", 140 },
                { "TL 143", "Tourist Train",150 },
                { "BS 46", "Electric Bus", 60 },
                { "FL 537", "Domestic Flight", 140 },
         });
            migrationBuilder.Sql("INSERT INTO AppUsers ( Username, Userpassword, FirstName, LastName, Email, Phone, UserRole) VALUES " +
          "('Admin123', '12345', 'Admin', '', 'admin@Booking.gmail.com', '8547233473', 0)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
