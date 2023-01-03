﻿// <auto-generated />
using System;
using CinemaCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CinemaCentral.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("CinemaCentral.Models.Episode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SeriesId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("SeriesId");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("CinemaCentral.Models.Genre", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("CinemaCentral.Models.Library", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Root")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Libraries");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ebf247de-d633-468d-86ca-f53080e622be"),
                            Name = "Movies",
                            Root = "/Users/justin/Workspace/PotatoDiet/Released/CinemaCentral/CinemaCentral/Libraries/Movies"
                        },
                        new
                        {
                            Id = new Guid("d32f71f5-9aaf-4d04-9bb7-8b0c045e6604"),
                            Name = "TV",
                            Root = "/Users/justin/Workspace/PotatoDiet/Released/CinemaCentral/CinemaCentral/Libraries/Series"
                        });
                });

            modelBuilder.Entity("CinemaCentral.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<float>("CommunityRating")
                        .HasColumnType("REAL");

                    b.Property<Guid>("LibraryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.HasIndex("Title");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("CinemaCentral.Models.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PosterPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SeriesId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("CinemaCentral.Models.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<float>("CommunityRating")
                        .HasColumnType("REAL");

                    b.Property<Guid>("LibraryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TmdbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.HasIndex("Title");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("CinemaCentral.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f807c6f7-3825-43c4-b48e-db0eb5928b58"),
                            Name = "user",
                            PasswordHash = new byte[] { 193, 108, 34, 212, 246, 137, 253, 1, 201, 177, 143, 159, 215, 52, 190, 79, 246, 175, 52, 99, 4, 80, 135, 190, 118, 8, 133, 202, 181, 254, 123, 125, 174, 74, 206, 167, 32, 154, 150, 140, 209, 200, 5, 198, 33, 206, 249, 24, 10, 81, 163, 248, 200, 117, 89, 212, 81, 245, 200, 234, 220, 163, 132, 84, 168, 187, 43, 96, 217, 208, 177, 87, 91, 69, 74, 71, 169, 130, 159, 79, 35, 239, 243, 31, 94, 186, 113, 191, 250, 79, 210, 213, 237, 151, 245, 50, 185, 175, 190, 11, 198, 154, 165, 174, 152, 14, 231, 144, 127, 141, 149, 165, 4, 235, 155, 161, 162, 52, 3, 56, 140, 234, 77, 111, 71, 12, 30, 251 },
                            PasswordSalt = new byte[] { 92, 86, 0, 136, 107, 55, 2, 234, 139, 169, 227, 91, 85, 96, 185, 113, 146, 176, 237, 180, 125, 8, 179, 124, 12, 93, 82, 243, 141, 116, 211, 77 },
                            Role = 0
                        });
                });

            modelBuilder.Entity("CinemaCentral.Models.WatchtimeStamp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("EpisodeId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MovieId")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Time")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EpisodeId");

                    b.HasIndex("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("WatchtimeStamps");
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.Property<string>("GenresName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MoviesId")
                        .HasColumnType("TEXT");

                    b.HasKey("GenresName", "MoviesId");

                    b.HasIndex("MoviesId");

                    b.ToTable("GenreMovie");
                });

            modelBuilder.Entity("GenreSeries", b =>
                {
                    b.Property<string>("GenresName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SeriesId")
                        .HasColumnType("TEXT");

                    b.HasKey("GenresName", "SeriesId");

                    b.HasIndex("SeriesId");

                    b.ToTable("GenreSeries");
                });

            modelBuilder.Entity("CinemaCentral.Models.Episode", b =>
                {
                    b.HasOne("CinemaCentral.Models.Season", "Season")
                        .WithMany("Episodes")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CinemaCentral.Models.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("CinemaCentral.Models.Movie", b =>
                {
                    b.HasOne("CinemaCentral.Models.Library", "Library")
                        .WithMany("Movies")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("CinemaCentral.Models.Season", b =>
                {
                    b.HasOne("CinemaCentral.Models.Series", "Series")
                        .WithMany("Seasons")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");
                });

            modelBuilder.Entity("CinemaCentral.Models.Series", b =>
                {
                    b.HasOne("CinemaCentral.Models.Library", "Library")
                        .WithMany("Series")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("CinemaCentral.Models.WatchtimeStamp", b =>
                {
                    b.HasOne("CinemaCentral.Models.Episode", "Episode")
                        .WithMany("WatchtimeStamps")
                        .HasForeignKey("EpisodeId");

                    b.HasOne("CinemaCentral.Models.Movie", "Movie")
                        .WithMany("WatchtimeStamps")
                        .HasForeignKey("MovieId");

                    b.HasOne("CinemaCentral.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Episode");

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.HasOne("CinemaCentral.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CinemaCentral.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GenreSeries", b =>
                {
                    b.HasOne("CinemaCentral.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CinemaCentral.Models.Series", null)
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CinemaCentral.Models.Episode", b =>
                {
                    b.Navigation("WatchtimeStamps");
                });

            modelBuilder.Entity("CinemaCentral.Models.Library", b =>
                {
                    b.Navigation("Movies");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("CinemaCentral.Models.Movie", b =>
                {
                    b.Navigation("WatchtimeStamps");
                });

            modelBuilder.Entity("CinemaCentral.Models.Season", b =>
                {
                    b.Navigation("Episodes");
                });

            modelBuilder.Entity("CinemaCentral.Models.Series", b =>
                {
                    b.Navigation("Seasons");
                });
#pragma warning restore 612, 618
        }
    }
}
