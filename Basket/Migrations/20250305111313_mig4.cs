using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basket.API.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaskettID",
                table: "BasketItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BaskettID",
                table: "BasketItems",
                column: "BaskettID");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Baskets_BaskettID",
                table: "BasketItems",
                column: "BaskettID",
                principalTable: "Baskets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Baskets_BaskettID",
                table: "BasketItems");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_BaskettID",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "BaskettID",
                table: "BasketItems");
        }
    }
}
