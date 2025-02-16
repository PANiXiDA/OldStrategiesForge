using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDataService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competencies_SubFactions_subfaction_id",
                table: "Competencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Heroes_SubFactions_subfaction_id",
                table: "Heroes");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Competencies_CompetenceId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_SubFactions_subfaction_id",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFactionAndAbilityScopes_Abilities_ability_id",
                table: "SubFactionAndAbilityScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFactionAndAbilityScopes_SubFactions_subfaction_id",
                table: "SubFactionAndAbilityScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubFactions_Factions_faction_id",
                table: "SubFactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubFactions",
                table: "SubFactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubFactionAndAbilityScopes",
                table: "SubFactionAndAbilityScopes");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CompetenceId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CompetenceId",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "SubFactions",
                newName: "Subfactions");

            migrationBuilder.RenameTable(
                name: "SubFactionAndAbilityScopes",
                newName: "SubfactionAndAbilityScopes");

            migrationBuilder.RenameIndex(
                name: "IX_SubFactions_faction_id",
                table: "Subfactions",
                newName: "IX_Subfactions_faction_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubFactionAndAbilityScopes_subfaction_id",
                table: "SubfactionAndAbilityScopes",
                newName: "IX_SubfactionAndAbilityScopes_subfaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubFactionAndAbilityScopes_ability_id",
                table: "SubfactionAndAbilityScopes",
                newName: "IX_SubfactionAndAbilityScopes_ability_id");

            migrationBuilder.RenameColumn(
                name: "Competence_id",
                table: "Skills",
                newName: "competence_id");

            migrationBuilder.RenameColumn(
                name: "artifact_slot",
                table: "Artefacts",
                newName: "artefact_slot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subfactions",
                table: "Subfactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubfactionAndAbilityScopes",
                table: "SubfactionAndAbilityScopes",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_competence_id",
                table: "Skills",
                column: "competence_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Competencies_Subfactions_subfaction_id",
                table: "Competencies",
                column: "subfaction_id",
                principalTable: "Subfactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Heroes_Subfactions_subfaction_id",
                table: "Heroes",
                column: "subfaction_id",
                principalTable: "Subfactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Competencies_competence_id",
                table: "Skills",
                column: "competence_id",
                principalTable: "Competencies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Subfactions_subfaction_id",
                table: "Skills",
                column: "subfaction_id",
                principalTable: "Subfactions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubfactionAndAbilityScopes_Abilities_ability_id",
                table: "SubfactionAndAbilityScopes",
                column: "ability_id",
                principalTable: "Abilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubfactionAndAbilityScopes_Subfactions_subfaction_id",
                table: "SubfactionAndAbilityScopes",
                column: "subfaction_id",
                principalTable: "Subfactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subfactions_Factions_faction_id",
                table: "Subfactions",
                column: "faction_id",
                principalTable: "Factions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competencies_Subfactions_subfaction_id",
                table: "Competencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Heroes_Subfactions_subfaction_id",
                table: "Heroes");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Competencies_competence_id",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Subfactions_subfaction_id",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_SubfactionAndAbilityScopes_Abilities_ability_id",
                table: "SubfactionAndAbilityScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_SubfactionAndAbilityScopes_Subfactions_subfaction_id",
                table: "SubfactionAndAbilityScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subfactions_Factions_faction_id",
                table: "Subfactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subfactions",
                table: "Subfactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubfactionAndAbilityScopes",
                table: "SubfactionAndAbilityScopes");

            migrationBuilder.DropIndex(
                name: "IX_Skills_competence_id",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "Subfactions",
                newName: "SubFactions");

            migrationBuilder.RenameTable(
                name: "SubfactionAndAbilityScopes",
                newName: "SubFactionAndAbilityScopes");

            migrationBuilder.RenameIndex(
                name: "IX_Subfactions_faction_id",
                table: "SubFactions",
                newName: "IX_SubFactions_faction_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubfactionAndAbilityScopes_subfaction_id",
                table: "SubFactionAndAbilityScopes",
                newName: "IX_SubFactionAndAbilityScopes_subfaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubfactionAndAbilityScopes_ability_id",
                table: "SubFactionAndAbilityScopes",
                newName: "IX_SubFactionAndAbilityScopes_ability_id");

            migrationBuilder.RenameColumn(
                name: "competence_id",
                table: "Skills",
                newName: "Competence_id");

            migrationBuilder.RenameColumn(
                name: "artefact_slot",
                table: "Artefacts",
                newName: "artifact_slot");

            migrationBuilder.AddColumn<int>(
                name: "CompetenceId",
                table: "Skills",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubFactions",
                table: "SubFactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubFactionAndAbilityScopes",
                table: "SubFactionAndAbilityScopes",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CompetenceId",
                table: "Skills",
                column: "CompetenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competencies_SubFactions_subfaction_id",
                table: "Competencies",
                column: "subfaction_id",
                principalTable: "SubFactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Heroes_SubFactions_subfaction_id",
                table: "Heroes",
                column: "subfaction_id",
                principalTable: "SubFactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Competencies_CompetenceId",
                table: "Skills",
                column: "CompetenceId",
                principalTable: "Competencies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_SubFactions_subfaction_id",
                table: "Skills",
                column: "subfaction_id",
                principalTable: "SubFactions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubFactionAndAbilityScopes_Abilities_ability_id",
                table: "SubFactionAndAbilityScopes",
                column: "ability_id",
                principalTable: "Abilities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubFactionAndAbilityScopes_SubFactions_subfaction_id",
                table: "SubFactionAndAbilityScopes",
                column: "subfaction_id",
                principalTable: "SubFactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubFactions_Factions_faction_id",
                table: "SubFactions",
                column: "faction_id",
                principalTable: "Factions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
