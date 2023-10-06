using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HHPW_Serverside.Migrations
{
    public partial class UserOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_orders_userId",
                table: "orders",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_userId",
                table: "orders",
                column: "userId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_userId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_userId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "orders");
        }
    }
}
