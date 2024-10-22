using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDecimalRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultChanceToSendMessage",
                table: "UserSettings",
                type: "decimal(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChanceToSaveTextMessage",
                table: "UserSettings",
                type: "decimal(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChanceToSaveMessage",
                table: "UserSettings",
                type: "decimal(6,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultChanceToSendMessage",
                table: "UserSettings",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChanceToSaveTextMessage",
                table: "UserSettings",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChanceToSaveMessage",
                table: "UserSettings",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,5)");
        }
    }
}
