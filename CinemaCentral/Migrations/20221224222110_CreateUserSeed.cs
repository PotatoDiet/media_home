using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaCentral.Migrations
{
    public partial class CreateUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"), "user", new byte[] { 120, 144, 248, 146, 96, 210, 141, 133, 52, 63, 197, 82, 38, 59, 233, 94, 195, 224, 255, 65, 239, 172, 106, 247, 216, 149, 60, 51, 136, 237, 13, 57, 224, 106, 119, 101, 100, 177, 236, 221, 10, 238, 66, 97, 104, 86, 244, 62, 77, 29, 41, 44, 216, 178, 62, 116, 254, 161, 162, 56, 174, 171, 23, 122, 165, 33, 216, 96, 9, 183, 72, 201, 108, 16, 77, 225, 42, 15, 14, 146, 168, 131, 217, 223, 17, 19, 143, 78, 195, 228, 112, 1, 93, 248, 242, 34, 23, 92, 228, 188, 247, 133, 132, 99, 40, 170, 142, 140, 82, 150, 244, 155, 45, 129, 182, 96, 198, 210, 26, 194, 31, 27, 99, 236, 223, 221, 115, 239 }, new byte[] { 242, 171, 124, 177, 160, 166, 25, 130, 175, 79, 201, 22, 96, 189, 39, 203, 129, 204, 27, 249, 77, 92, 186, 86, 166, 249, 182, 38, 97, 24, 168, 149 }, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"));
        }
    }
}
