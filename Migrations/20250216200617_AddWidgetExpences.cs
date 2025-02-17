using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssistent.Migrations
{
    /// <inheritdoc />
    public partial class AddWidgetExpences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Expenses",
                table: "Widgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expenses",
                table: "Widgets");
        }
    }
}
