using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProfileService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFrames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frames",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    s3_path = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    necessary_mmr = table.Column<int>(type: "integer", nullable: false),
                    necessary_games = table.Column<int>(type: "integer", nullable: false),
                    necessary_wins = table.Column<int>(type: "integer", nullable: false),
                    available = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frames", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_avatar_id",
                table: "Players",
                column: "avatar_id");

            migrationBuilder.CreateIndex(
                name: "IX_Players_frame_id",
                table: "Players",
                column: "frame_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Avatars_avatar_id",
                table: "Players",
                column: "avatar_id",
                principalTable: "Avatars",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Frames_frame_id",
                table: "Players",
                column: "frame_id",
                principalTable: "Frames",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Avatars_avatar_id",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Frames_frame_id",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Frames");

            migrationBuilder.DropIndex(
                name: "IX_Players_avatar_id",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_frame_id",
                table: "Players");
        }
    }
}
