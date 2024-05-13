using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketReservationDataAccess.Data.Migrations
{
    public partial class keyIdentityandvaluestoentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO Destinations (Destination_name) VALUES
            ('Delhi'),
            ('Mumbai'),
            ('Bangalore'),
            ('Kolkata'),
            ('Chennai'),
            ('Hyderabad'),
            ('Jaipur'),
            ('Ahmedabad');
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
