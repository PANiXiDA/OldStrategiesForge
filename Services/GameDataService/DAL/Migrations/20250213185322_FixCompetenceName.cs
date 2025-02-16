using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDataService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixCompetenceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Competencies_competency_id",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_competency_id",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "competency_id",
                table: "Skills",
                newName: "Competence_id");

            migrationBuilder.AddColumn<int>(
                name: "CompetenceId",
                table: "Skills",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CompetenceId",
                table: "Skills",
                column: "CompetenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Competencies_CompetenceId",
                table: "Skills",
                column: "CompetenceId",
                principalTable: "Competencies",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Competencies_CompetenceId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CompetenceId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CompetenceId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "Competence_id",
                table: "Skills",
                newName: "competency_id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_competency_id",
                table: "Skills",
                column: "competency_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Competencies_competency_id",
                table: "Skills",
                column: "competency_id",
                principalTable: "Competencies",
                principalColumn: "id");
        }
    }
}
