using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBot.DAL.Migrations
{
    public partial class PoolisClosed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isClosed",
                table: "Polls",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isClosed",
                table: "Polls");
        }
    }
}
