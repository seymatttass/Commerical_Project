using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Address.API.Migrations
{
    /// <inheritdoc />
    public partial class migg_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Town",
                table: "Addres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Town",
                table: "Addres",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
