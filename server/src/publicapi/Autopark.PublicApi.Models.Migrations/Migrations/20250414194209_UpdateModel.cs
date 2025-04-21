using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.PublicApi.Models.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permissions_VehicleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Odometer",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "FuelTypeId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MaintenanceRecords",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OdometerHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdometerHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdometerHistories_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ServiceLife = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefuelingHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefuelingHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefuelingHistories_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventHistories_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventHistories_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartReplacements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Odometer = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartReplacements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartReplacements_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartReplacements_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleStatusHistories_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleStatusHistories_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_FuelTypeId",
                table: "Vehicles",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_VehicleId",
                table: "Permissions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_EventHistories_EventTypeId",
                table: "EventHistories",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventHistories_VehicleId",
                table: "EventHistories",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_OdometerHistories_VehicleId",
                table: "OdometerHistories",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PartReplacements_PartId",
                table: "PartReplacements",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartReplacements_VehicleId",
                table: "PartReplacements",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefuelingHistories_VehicleId",
                table: "RefuelingHistories",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStatusHistories_StatusId",
                table: "VehicleStatusHistories",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStatusHistories_VehicleId",
                table: "VehicleStatusHistories",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_FuelTypes_FuelTypeId",
                table: "Vehicles",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_FuelTypes_FuelTypeId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "EventHistories");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "OdometerHistories");

            migrationBuilder.DropTable(
                name: "PartReplacements");

            migrationBuilder.DropTable(
                name: "RefuelingHistories");

            migrationBuilder.DropTable(
                name: "VehicleStatusHistories");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_FuelTypeId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_VehicleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "FuelTypeId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MaintenanceRecords");

            migrationBuilder.AddColumn<long>(
                name: "Odometer",
                table: "Vehicles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Vehicles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_VehicleId",
                table: "Permissions",
                column: "VehicleId",
                unique: true);
        }
    }
}
