using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssistent.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_AspNetUsers_UserId",
                table: "Widgets");

            migrationBuilder.DropIndex(
                name: "IX_Widgets_UserId",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Cash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MonthlyBudget",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "Widgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UsersInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeeklyCostsLimits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailyCostsLimits = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersInfo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_UserInfoId",
                table: "Widgets",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInfo_UserId",
                table: "UsersInfo",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_UsersInfo_UserInfoId",
                table: "Widgets",
                column: "UserInfoId",
                principalTable: "UsersInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_UsersInfo_UserInfoId",
                table: "Widgets");

            migrationBuilder.DropTable(
                name: "UsersInfo");

            migrationBuilder.DropIndex(
                name: "IX_Widgets_UserInfoId",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Widgets");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Widgets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Cash",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyBudget",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_UserId",
                table: "Widgets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_AspNetUsers_UserId",
                table: "Widgets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
