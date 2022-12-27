using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaCentral.Migrations
{
    public partial class CleanModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Seasons_SeasonId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "CurrentWatchTimestamp",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CurrentWatchTimestamp",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "SeasonNumber",
                table: "Episodes");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Movies",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SeasonId",
                table: "Episodes",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Episodes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 112, 104, 157, 142, 34, 123, 71, 40, 167, 140, 69, 253, 83, 141, 68, 148, 53, 31, 95, 65, 27, 66, 200, 120, 95, 207, 173, 154, 137, 120, 213, 40, 153, 40, 60, 174, 92, 145, 188, 187, 147, 132, 12, 144, 78, 161, 231, 8, 206, 198, 230, 95, 175, 59, 116, 28, 43, 31, 251, 155, 158, 135, 71, 201, 79, 172, 106, 5, 214, 195, 217, 136, 226, 242, 94, 161, 70, 187, 41, 234, 192, 215, 211, 153, 66, 42, 104, 12, 202, 39, 238, 131, 234, 69, 199, 145, 48, 166, 131, 134, 191, 74, 196, 220, 12, 244, 226, 191, 181, 125, 36, 168, 47, 168, 3, 104, 194, 2, 94, 198, 171, 185, 77, 126, 120, 23, 113, 148 }, new byte[] { 102, 185, 74, 78, 22, 230, 21, 120, 192, 147, 231, 202, 225, 79, 130, 178, 36, 219, 91, 7, 30, 120, 111, 253, 10, 173, 169, 232, 220, 209, 128, 95 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Seasons_SeasonId",
                table: "Episodes",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Seasons_SeasonId",
                table: "Episodes");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Movies",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<uint>(
                name: "CurrentWatchTimestamp",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<Guid>(
                name: "SeasonId",
                table: "Episodes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Episodes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<uint>(
                name: "CurrentWatchTimestamp",
                table: "Episodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<int>(
                name: "SeasonNumber",
                table: "Episodes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 120, 144, 248, 146, 96, 210, 141, 133, 52, 63, 197, 82, 38, 59, 233, 94, 195, 224, 255, 65, 239, 172, 106, 247, 216, 149, 60, 51, 136, 237, 13, 57, 224, 106, 119, 101, 100, 177, 236, 221, 10, 238, 66, 97, 104, 86, 244, 62, 77, 29, 41, 44, 216, 178, 62, 116, 254, 161, 162, 56, 174, 171, 23, 122, 165, 33, 216, 96, 9, 183, 72, 201, 108, 16, 77, 225, 42, 15, 14, 146, 168, 131, 217, 223, 17, 19, 143, 78, 195, 228, 112, 1, 93, 248, 242, 34, 23, 92, 228, 188, 247, 133, 132, 99, 40, 170, 142, 140, 82, 150, 244, 155, 45, 129, 182, 96, 198, 210, 26, 194, 31, 27, 99, 236, 223, 221, 115, 239 }, new byte[] { 242, 171, 124, 177, 160, 166, 25, 130, 175, 79, 201, 22, 96, 189, 39, 203, 129, 204, 27, 249, 77, 92, 186, 86, 166, 249, 182, 38, 97, 24, 168, 149 } });

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Seasons_SeasonId",
                table: "Episodes",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");
        }
    }
}
