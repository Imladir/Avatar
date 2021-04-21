using Microsoft.EntityFrameworkCore.Migrations;

namespace AvatarBot.Model.Migrations
{
    public partial class Localization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiceChannelID",
                table: "Servers");

            migrationBuilder.AddColumn<string>(
                name: "Localization",
                table: "Servers",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Localization",
                table: "Servers");

            migrationBuilder.AddColumn<long>(
                name: "DiceChannelID",
                table: "Servers",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
