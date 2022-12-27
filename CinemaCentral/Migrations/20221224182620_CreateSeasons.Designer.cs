﻿// <auto-generated />
using System;
using CinemaCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CinemaCentral.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221224182620_CreateSeasons")]
    partial class CreateSeasons
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("CinemaCentral.Models.Episode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<uint>("CurrentWatchTimestamp")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SeasonId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

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

                    b.Property<Guid?>("SeriesId")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.HasIndex("SeriesId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("CinemaCentral.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<float>("CommunityRating")
                        .HasColumnType("REAL");

                    b.Property<uint>("CurrentWatchTimestamp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
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

            modelBuilder.Entity("CinemaCentral.Models.Episode", b =>
                {
                    b.HasOne("CinemaCentral.Models.Season", null)
                        .WithMany("Episodes")
                        .HasForeignKey("SeasonId");

                    b.HasOne("CinemaCentral.Models.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");
                });

            modelBuilder.Entity("CinemaCentral.Models.Genre", b =>
                {
                    b.HasOne("CinemaCentral.Models.Series", null)
                        .WithMany("Genres")
                        .HasForeignKey("SeriesId");
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

            modelBuilder.Entity("CinemaCentral.Models.Episode", b =>
                {
                    b.Navigation("WatchtimeStamps");
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
                    b.Navigation("Genres");

                    b.Navigation("Seasons");
                });
#pragma warning restore 612, 618
        }
    }
}
