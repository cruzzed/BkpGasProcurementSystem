using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BkpGasProcurementSystem.Migrations.BkpGasProcurementSystemDeliveries
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ordersID",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_date = table.Column<DateTime>(nullable: false),
                    username = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    total_price = table.Column<float>(nullable: false),
                    Payment_status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<byte[]>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    OrdersID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Orders_OrdersID",
                        column: x => x.OrdersID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ordersID",
                table: "Deliveries",
                column: "ordersID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_OrdersID",
                table: "Product",
                column: "OrdersID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Orders_ordersID",
                table: "Deliveries",
                column: "ordersID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Orders_ordersID",
                table: "Deliveries");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_ordersID",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ordersID",
                table: "Deliveries");
        }
    }
}
