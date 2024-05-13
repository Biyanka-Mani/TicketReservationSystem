using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketReservationDataAccess.Data.Migrations
{
    public partial class FoodPreferencesRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodPreference",
                table: "AppUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FoodPreference",
                table: "AppUsers",
                type: "bit",
                nullable: true);
        }
    }
}
