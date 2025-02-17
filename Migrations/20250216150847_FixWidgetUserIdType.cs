using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAssistent.Migrations
{
    /// <inheritdoc />
    public partial class FixWidgetUserIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WidgetEntity_AspNetUsers_UserId1",
                table: "WidgetEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_WidgetEntity_IconEntity_IconID",
                table: "WidgetEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WidgetEntity",
                table: "WidgetEntity");

            migrationBuilder.DropIndex(
                name: "IX_WidgetEntity_UserId1",
                table: "WidgetEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IconEntity",
                table: "IconEntity");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "WidgetEntity");

            migrationBuilder.RenameTable(
                name: "WidgetEntity",
                newName: "Widgets");

            migrationBuilder.RenameTable(
                name: "IconEntity",
                newName: "Icons");

            migrationBuilder.RenameIndex(
                name: "IX_WidgetEntity_IconID",
                table: "Widgets",
                newName: "IX_Widgets_IconID");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Widgets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Widgets",
                table: "Widgets",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Icons",
                table: "Icons",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Widgets_Icons_IconID",
                table: "Widgets",
                column: "IconID",
                principalTable: "Icons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_AspNetUsers_UserId",
                table: "Widgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Widgets_Icons_IconID",
                table: "Widgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Widgets",
                table: "Widgets");

            migrationBuilder.DropIndex(
                name: "IX_Widgets_UserId",
                table: "Widgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Icons",
                table: "Icons");

            migrationBuilder.RenameTable(
                name: "Widgets",
                newName: "WidgetEntity");

            migrationBuilder.RenameTable(
                name: "Icons",
                newName: "IconEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Widgets_IconID",
                table: "WidgetEntity",
                newName: "IX_WidgetEntity_IconID");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "WidgetEntity",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "WidgetEntity",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WidgetEntity",
                table: "WidgetEntity",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IconEntity",
                table: "IconEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WidgetEntity_UserId1",
                table: "WidgetEntity",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WidgetEntity_AspNetUsers_UserId1",
                table: "WidgetEntity",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WidgetEntity_IconEntity_IconID",
                table: "WidgetEntity",
                column: "IconID",
                principalTable: "IconEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
