using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaCentral.Migrations
{
    public partial class CreateSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Genres",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Overview = table.Column<string>(type: "TEXT", nullable: false),
                    PosterPath = table.Column<string>(type: "TEXT", nullable: true),
                    CommunityRating = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    PosterPath = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentWatchTimestamp = table.Column<uint>(type: "INTEGER", nullable: false),
                    SeriesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_SeriesId",
                table: "Genres",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SeriesId",
                table: "Episodes",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Title",
                table: "Series",
                column: "Title");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Series_SeriesId",
                table: "Genres",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Series_SeriesId",
                table: "Genres");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Genres_SeriesId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Genres");
        }
    }
}
