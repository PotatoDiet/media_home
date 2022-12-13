using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaCentral.Migrations
{
    public partial class AddTvmdbIdToSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Series",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Series");
        }
    }
}
