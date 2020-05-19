using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarChecker.Server.Data.Migrations
{
    public partial class Vehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    LicenseNumber = table.Column<string>(nullable: false),
                    Make = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Mileage = table.Column<int>(nullable: false),
                    Tank = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.LicenseNumber);
                });

            migrationBuilder.CreateTable(
                name: "InspectionNote",
                columns: table => new
                {
                    InspectionNoteId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Location = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 100, nullable: false),
                    PhotoUrl = table.Column<string>(nullable: true),
                    VehicleLicenseNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionNote", x => x.InspectionNoteId);
                    table.ForeignKey(
                        name: "FK_InspectionNote_Vehicles_VehicleLicenseNumber",
                        column: x => x.VehicleLicenseNumber,
                        principalTable: "Vehicles",
                        principalColumn: "LicenseNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionNote_VehicleLicenseNumber",
                table: "InspectionNote",
                column: "VehicleLicenseNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionNote");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
