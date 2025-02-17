using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssistent.Migrations
{
    /// <inheritdoc />
    public partial class AddWidgets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IconEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IconEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WidgetEntity",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconID = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetEntity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WidgetEntity_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WidgetEntity_IconEntity_IconID",
                        column: x => x.IconID,
                        principalTable: "IconEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WidgetEntity_IconID",
                table: "WidgetEntity",
                column: "IconID");

            migrationBuilder.CreateIndex(
                name: "IX_WidgetEntity_UserId1",
                table: "WidgetEntity",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WidgetEntity");

            migrationBuilder.DropTable(
                name: "IconEntity");
        }
    }
}
