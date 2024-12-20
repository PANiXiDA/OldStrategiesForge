using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatsService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerColumnsToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_s3_path",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "frame_s3_path",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sender_nickname",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_s3_path",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "frame_s3_path",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "sender_nickname",
                table: "Messages");
        }
    }
}
