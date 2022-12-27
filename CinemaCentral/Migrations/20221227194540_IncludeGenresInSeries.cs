using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaCentral.Migrations
{
    public partial class IncludeGenresInSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Series_SeriesId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_SeriesId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Genres");

            migrationBuilder.CreateTable(
                name: "GenreSeries",
                columns: table => new
                {
                    GenresName = table.Column<string>(type: "TEXT", nullable: false),
                    SeriesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreSeries", x => new { x.GenresName, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_GenreSeries_Genres_GenresName",
                        column: x => x.GenresName,
                        principalTable: "Genres",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 101, 6, 129, 237, 185, 234, 68, 69, 246, 188, 150, 234, 135, 151, 133, 106, 110, 213, 140, 187, 116, 162, 197, 33, 194, 233, 97, 9, 174, 47, 147, 92, 233, 227, 153, 170, 148, 183, 183, 219, 212, 248, 9, 129, 233, 73, 44, 103, 109, 68, 50, 195, 164, 136, 224, 193, 164, 129, 187, 99, 173, 200, 249, 23, 182, 223, 63, 82, 90, 75, 27, 227, 200, 2, 122, 161, 49, 72, 138, 102, 120, 7, 167, 83, 179, 107, 100, 77, 115, 127, 217, 52, 188, 30, 150, 1, 109, 7, 6, 188, 214, 11, 2, 246, 70, 223, 100, 29, 14, 172, 58, 154, 221, 16, 229, 183, 28, 57, 15, 230, 38, 152, 173, 165, 72, 160, 104, 147 }, new byte[] { 21, 117, 195, 114, 132, 27, 168, 103, 228, 95, 122, 60, 45, 31, 202, 2, 221, 116, 100, 40, 9, 62, 20, 94, 91, 75, 63, 33, 0, 255, 154, 32 } });

            migrationBuilder.CreateIndex(
                name: "IX_GenreSeries_SeriesId",
                table: "GenreSeries",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreSeries");

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Genres",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 112, 104, 157, 142, 34, 123, 71, 40, 167, 140, 69, 253, 83, 141, 68, 148, 53, 31, 95, 65, 27, 66, 200, 120, 95, 207, 173, 154, 137, 120, 213, 40, 153, 40, 60, 174, 92, 145, 188, 187, 147, 132, 12, 144, 78, 161, 231, 8, 206, 198, 230, 95, 175, 59, 116, 28, 43, 31, 251, 155, 158, 135, 71, 201, 79, 172, 106, 5, 214, 195, 217, 136, 226, 242, 94, 161, 70, 187, 41, 234, 192, 215, 211, 153, 66, 42, 104, 12, 202, 39, 238, 131, 234, 69, 199, 145, 48, 166, 131, 134, 191, 74, 196, 220, 12, 244, 226, 191, 181, 125, 36, 168, 47, 168, 3, 104, 194, 2, 94, 198, 171, 185, 77, 126, 120, 23, 113, 148 }, new byte[] { 102, 185, 74, 78, 22, 230, 21, 120, 192, 147, 231, 202, 225, 79, 130, 178, 36, 219, 91, 7, 30, 120, 111, 253, 10, 173, 169, 232, 220, 209, 128, 95 } });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_SeriesId",
                table: "Genres",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Series_SeriesId",
                table: "Genres",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }
    }
}
