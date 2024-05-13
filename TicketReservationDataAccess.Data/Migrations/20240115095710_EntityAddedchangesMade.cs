using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketReservationDataAccess.Data.Migrations
{
    public partial class EntityAddedchangesMade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalDestination",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DepatureDestination",
                table: "Routes");

         

            migrationBuilder.AddColumn<int>(
                name: "ArrivalDestinationId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepatureDestinationId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destination_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ArrivalDestinationId",
                table: "Routes",
                column: "ArrivalDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DepatureDestinationId",
                table: "Routes",
                column: "DepatureDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_ArrivalDestinationId",
                table: "Routes",
                column: "ArrivalDestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_DepatureDestinationId",
                table: "Routes",
                column: "DepatureDestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_ArrivalDestinationId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_DepatureDestinationId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ArrivalDestinationId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_DepatureDestinationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ArrivalDestinationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DepatureDestinationId",
                table: "Routes");

            migrationBuilder.AddColumn<string>(
                name: "ArrivalDestination",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepatureDestination",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            
        }
    }
}
