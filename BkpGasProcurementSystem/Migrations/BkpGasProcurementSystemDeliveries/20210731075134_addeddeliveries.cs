using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BkpGasProcurementSystem.Migrations.BkpGasProcurementSystemDeliveries
{
    public partial class addeddeliveries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(nullable: true),
                    ship_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "update_delivery",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(nullable: true),
                    update_when = table.Column<DateTime>(nullable: false),
                    status = table.Column<string>(nullable: true),
                    DeliveriesID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_update_delivery", x => x.ID);
                    table.ForeignKey(
                        name: "FK_update_delivery_Deliveries_DeliveriesID",
                        column: x => x.DeliveriesID,
                        principalTable: "Deliveries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_update_delivery_DeliveriesID",
                table: "update_delivery",
                column: "DeliveriesID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "update_delivery");

            migrationBuilder.DropTable(
                name: "Deliveries");
        }
    }
}
