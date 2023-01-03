using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaCentral.Migrations
{
    public partial class AddLibraries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LibraryId",
                table: "Series",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LibraryId",
                table: "Movies",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Root = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Libraries",
                columns: new[] { "Id", "Name", "Root" },
                values: new object[] { new Guid("d32f71f5-9aaf-4d04-9bb7-8b0c045e6604"), "TV", "/Users/justin/Workspace/PotatoDiet/Released/CinemaCentral/CinemaCentral/Libraries/Series" });

            migrationBuilder.InsertData(
                table: "Libraries",
                columns: new[] { "Id", "Name", "Root" },
                values: new object[] { new Guid("ebf247de-d633-468d-86ca-f53080e622be"), "Movies", "/Users/justin/Workspace/PotatoDiet/Released/CinemaCentral/CinemaCentral/Libraries/Movies" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 88, 85, 101, 95, 121, 50, 77, 113, 92, 188, 68, 155, 68, 67, 43, 152, 161, 136, 159, 11, 96, 217, 5, 207, 2, 114, 97, 177, 189, 82, 0, 249, 78, 248, 147, 181, 4, 2, 229, 27, 246, 124, 63, 41, 94, 120, 95, 106, 208, 196, 219, 30, 246, 214, 128, 212, 211, 51, 116, 77, 183, 58, 204, 255, 47, 81, 27, 235, 172, 160, 137, 210, 60, 77, 183, 247, 61, 38, 41, 107, 2, 237, 181, 85, 52, 84, 200, 228, 73, 218, 99, 21, 244, 122, 105, 26, 243, 90, 60, 73, 236, 138, 15, 147, 16, 98, 142, 249, 158, 206, 194, 44, 57, 13, 77, 45, 132, 193, 51, 238, 189, 214, 129, 93, 111, 114, 131, 188 }, new byte[] { 253, 218, 66, 196, 236, 158, 9, 13, 147, 227, 15, 26, 193, 176, 198, 61, 82, 167, 198, 117, 182, 198, 11, 45, 135, 204, 242, 229, 52, 51, 219, 140 } });

            migrationBuilder.CreateIndex(
                name: "IX_Series_LibraryId",
                table: "Series",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_LibraryId",
                table: "Movies",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Libraries_LibraryId",
                table: "Movies",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Libraries_LibraryId",
                table: "Series",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Libraries_LibraryId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Libraries_LibraryId",
                table: "Series");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Series_LibraryId",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Movies_LibraryId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "Movies");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 101, 6, 129, 237, 185, 234, 68, 69, 246, 188, 150, 234, 135, 151, 133, 106, 110, 213, 140, 187, 116, 162, 197, 33, 194, 233, 97, 9, 174, 47, 147, 92, 233, 227, 153, 170, 148, 183, 183, 219, 212, 248, 9, 129, 233, 73, 44, 103, 109, 68, 50, 195, 164, 136, 224, 193, 164, 129, 187, 99, 173, 200, 249, 23, 182, 223, 63, 82, 90, 75, 27, 227, 200, 2, 122, 161, 49, 72, 138, 102, 120, 7, 167, 83, 179, 107, 100, 77, 115, 127, 217, 52, 188, 30, 150, 1, 109, 7, 6, 188, 214, 11, 2, 246, 70, 223, 100, 29, 14, 172, 58, 154, 221, 16, 229, 183, 28, 57, 15, 230, 38, 152, 173, 165, 72, 160, 104, 147 }, new byte[] { 21, 117, 195, 114, 132, 27, 168, 103, 228, 95, 122, 60, 45, 31, 202, 2, 221, 116, 100, 40, 9, 62, 20, 94, 91, 75, 63, 33, 0, 255, 154, 32 } });
        }
    }
}
