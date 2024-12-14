﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProfileService.DAL.DbModels;

#nullable disable

namespace ProfileService.DAL.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    [Migration("20241214185802_AddFrames")]
    partial class AddFrames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProfileService.DAL.DbModels.Models.Avatar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Available")
                        .HasColumnType("boolean")
                        .HasColumnName("available");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("NecessaryGames")
                        .HasColumnType("integer")
                        .HasColumnName("necessary_games");

                    b.Property<int>("NecessaryMmr")
                        .HasColumnType("integer")
                        .HasColumnName("necessary_mmr");

                    b.Property<int>("NecessaryWins")
                        .HasColumnType("integer")
                        .HasColumnName("necessary_wins");

                    b.Property<string>("S3Path")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("s3_path");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("Avatars");
                });

            modelBuilder.Entity("ProfileService.DAL.DbModels.Models.Frame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Available")
                        .HasColumnType("boolean")
                        .HasColumnName("available");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("NecessaryGames")
                        .HasColumnType("integer")
                        .HasColumnName("necessary_games");

                    b.Property<int>("NecessaryMmr")
                        .HasColumnType("integer")
                        .HasColumnName("necessary_mmr");

                    b.Property<int>("NecessaryWins")
                        .HasColumnType("integer")
                        .HasColumnName("necessary_wins");

                    b.Property<string>("S3Path")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("s3_path");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("Frames");
                });

            modelBuilder.Entity("ProfileService.DAL.DbModels.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AvatarId")
                        .HasColumnType("integer")
                        .HasColumnName("avatar_id");

                    b.Property<bool>("Blocked")
                        .HasColumnType("boolean")
                        .HasColumnName("blocked");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("confirmed");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<int>("Experience")
                        .HasColumnType("integer")
                        .HasColumnName("experience");

                    b.Property<int>("FrameId")
                        .HasColumnType("integer")
                        .HasColumnName("frame_id");

                    b.Property<int>("Games")
                        .HasColumnType("integer")
                        .HasColumnName("games");

                    b.Property<int>("Gold")
                        .HasColumnType("integer")
                        .HasColumnName("gold");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_login");

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("level");

                    b.Property<int>("Loses")
                        .HasColumnType("integer")
                        .HasColumnName("loses");

                    b.Property<int>("Mmr")
                        .HasColumnType("integer")
                        .HasColumnName("mmr");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("nickname");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<bool>("Premium")
                        .HasColumnType("boolean")
                        .HasColumnName("premium");

                    b.Property<int>("Rank")
                        .HasColumnType("integer")
                        .HasColumnName("rank");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Wins")
                        .HasColumnType("integer")
                        .HasColumnName("wins");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("FrameId");

                    b.HasIndex("Nickname")
                        .IsUnique()
                        .HasDatabaseName("Unique_Users_Login");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ProfileService.DAL.DbModels.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer")
                        .HasColumnName("player_id");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime>("RefreshTokenExp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_exp");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("ProfileService.DAL.DbModels.Models.Player", b =>
                {
                    b.HasOne("ProfileService.DAL.DbModels.Models.Avatar", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProfileService.DAL.DbModels.Models.Frame", "Frame")
                        .WithMany()
                        .HasForeignKey("FrameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avatar");

                    b.Navigation("Frame");
                });

            modelBuilder.Entity("ProfileService.DAL.DbModels.Models.Token", b =>
                {
                    b.HasOne("ProfileService.DAL.DbModels.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
