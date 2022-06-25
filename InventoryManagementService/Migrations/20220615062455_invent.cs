using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementService.Migrations
{
    public partial class invent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Airlines",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        AirlineName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Airlines", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Flights",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FlightName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AirlineId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Flights", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Flights_Airlines_AirlineId",
            //            column: x => x.AirlineId,
            //            principalTable: "Airlines",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });
            migrationBuilder.AddColumn<int>(
             name: "IsActive",
             table: "FlightSchedules",
             nullable: true);

            migrationBuilder.Sql(
            @"UPDATE FlightSchedules SET IsActive =1");

            migrationBuilder.AlterColumn<int>(
                   name: "IsActive",
                   table: "FlightSchedules",
                   nullable: false);

            //migrationBuilder.CreateTable(
            //    name: "FlightSchedules",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FlightId = table.Column<int>(type: "int", nullable: false),
            //        AirLineId = table.Column<int>(type: "int", nullable: false),
            //        FromPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ToPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        TotalSeats = table.Column<int>(type: "int", nullable: false),
            //        BusinessClassSeats = table.Column<int>(type: "int", nullable: false),
            //        NonBusinessClassSeats = table.Column<int>(type: "int", nullable: false),
            //        BusinessTicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        NonBusinessTicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        MealOption = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsActive = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FlightSchedules", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_FlightSchedules_Airlines_AirLineId",
            //            column: x => x.AirLineId,
            //            principalTable: "Airlines",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_FlightSchedules_Flights_FlightId",
            //            column: x => x.FlightId,
            //            principalTable: "Flights",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });



            //migrationBuilder.CreateIndex(
            //    name: "IX_Flights_AirlineId",
            //    table: "Flights",
            //    column: "AirlineId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FlightSchedules_AirLineId",
            //    table: "FlightSchedules",
            //    column: "AirLineId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSchedules_FlightId",
                table: "FlightSchedules",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightSchedules");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
