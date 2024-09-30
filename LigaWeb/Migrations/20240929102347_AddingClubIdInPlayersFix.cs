using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddingClubIdInPlayersFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Stadiums_StadiumId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_StadiumId",
                table: "Clubs");

            migrationBuilder.AlterColumn<int>(
                name: "StadiumId",
                table: "Clubs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_StadiumId",
                table: "Clubs",
                column: "StadiumId",
                unique: true,
                filter: "[StadiumId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Stadiums_StadiumId",
                table: "Clubs",
                column: "StadiumId",
                principalTable: "Stadiums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Stadiums_StadiumId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_StadiumId",
                table: "Clubs");

            migrationBuilder.AlterColumn<int>(
                name: "StadiumId",
                table: "Clubs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_StadiumId",
                table: "Clubs",
                column: "StadiumId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Stadiums_StadiumId",
                table: "Clubs",
                column: "StadiumId",
                principalTable: "Stadiums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
