using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameDataService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ArtefactSets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtefactSets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Effects",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    effect_type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: true),
                    duration = table.Column<double>(type: "double precision", nullable: true),
                    parametrs = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Effects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "HeroClasses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroClasses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AbilityAndEffectScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ability_id = table.Column<int>(type: "integer", nullable: false),
                    effect_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityAndEffectScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_AbilityAndEffectScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityAndEffectScopes_Effects_effect_id",
                        column: x => x.effect_id,
                        principalTable: "Effects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    faction_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_FactionAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionAndAbilityScopes_Factions_faction_id",
                        column: x => x.faction_id,
                        principalTable: "Factions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubFactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    faction_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubFactions_Factions_faction_id",
                        column: x => x.faction_id,
                        principalTable: "Factions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    attack = table.Column<int>(type: "integer", nullable: false),
                    defence = table.Column<int>(type: "integer", nullable: false),
                    health = table.Column<int>(type: "integer", nullable: false),
                    min_damage = table.Column<int>(type: "integer", nullable: false),
                    max_damage = table.Column<int>(type: "integer", nullable: false),
                    initiative = table.Column<double>(type: "double precision", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    range = table.Column<int>(type: "integer", nullable: true),
                    arrows = table.Column<int>(type: "integer", nullable: true),
                    morale = table.Column<int>(type: "integer", nullable: false),
                    luck = table.Column<int>(type: "integer", nullable: false),
                    faction_id = table.Column<int>(type: "integer", nullable: true),
                    base_unit_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.id);
                    table.ForeignKey(
                        name: "FK_Units_Factions_faction_id",
                        column: x => x.faction_id,
                        principalTable: "Factions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Units_Units_base_unit_id",
                        column: x => x.base_unit_id,
                        principalTable: "Units",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Artefacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    attack_bonus = table.Column<int>(type: "integer", nullable: true),
                    defence_bonus = table.Column<int>(type: "integer", nullable: true),
                    health_bonus = table.Column<int>(type: "integer", nullable: true),
                    min_damage_bonus = table.Column<int>(type: "integer", nullable: true),
                    max_damage_bonus = table.Column<int>(type: "integer", nullable: true),
                    initiative_bonus = table.Column<double>(type: "double precision", nullable: true),
                    speed_bonus = table.Column<int>(type: "integer", nullable: true),
                    range_bonus = table.Column<int>(type: "integer", nullable: true),
                    arrows_bonus = table.Column<int>(type: "integer", nullable: true),
                    morale_bonus = table.Column<int>(type: "integer", nullable: true),
                    luck_bonus = table.Column<int>(type: "integer", nullable: true),
                    artifact_slot = table.Column<int>(type: "integer", nullable: false),
                    hero_class_id = table.Column<int>(type: "integer", nullable: true),
                    artefact_set_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artefacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Artefacts_ArtefactSets_artefact_set_id",
                        column: x => x.artefact_set_id,
                        principalTable: "ArtefactSets",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Artefacts_HeroClasses_hero_class_id",
                        column: x => x.hero_class_id,
                        principalTable: "HeroClasses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ArtefactSetBonuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attack_bonus = table.Column<int>(type: "integer", nullable: true),
                    defence_bonus = table.Column<int>(type: "integer", nullable: true),
                    health_bonus = table.Column<int>(type: "integer", nullable: true),
                    min_damage_bonus = table.Column<int>(type: "integer", nullable: true),
                    max_damage_bonus = table.Column<int>(type: "integer", nullable: true),
                    initiative_bonus = table.Column<double>(type: "double precision", nullable: true),
                    speed_bonus = table.Column<int>(type: "integer", nullable: true),
                    range_bonus = table.Column<int>(type: "integer", nullable: true),
                    arrows_bonus = table.Column<int>(type: "integer", nullable: true),
                    morale_bonus = table.Column<int>(type: "integer", nullable: true),
                    luck_bonus = table.Column<int>(type: "integer", nullable: true),
                    hero_class_id = table.Column<int>(type: "integer", nullable: true),
                    artefact_set_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtefactSetBonuses", x => x.id);
                    table.ForeignKey(
                        name: "FK_ArtefactSetBonuses_ArtefactSets_artefact_set_id",
                        column: x => x.artefact_set_id,
                        principalTable: "ArtefactSets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtefactSetBonuses_HeroClasses_hero_class_id",
                        column: x => x.hero_class_id,
                        principalTable: "HeroClasses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HeroClassAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hero_class_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroClassAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_HeroClassAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroClassAndAbilityScopes_HeroClasses_hero_class_id",
                        column: x => x.hero_class_id,
                        principalTable: "HeroClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    subfaction_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => x.id);
                    table.ForeignKey(
                        name: "FK_Competencies_SubFactions_subfaction_id",
                        column: x => x.subfaction_id,
                        principalTable: "SubFactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    attack = table.Column<int>(type: "integer", nullable: false),
                    defence = table.Column<int>(type: "integer", nullable: false),
                    min_damage = table.Column<int>(type: "integer", nullable: false),
                    max_damage = table.Column<int>(type: "integer", nullable: false),
                    initiative = table.Column<double>(type: "double precision", nullable: false),
                    morale = table.Column<int>(type: "integer", nullable: false),
                    luck = table.Column<int>(type: "integer", nullable: false),
                    subfaction_id = table.Column<int>(type: "integer", nullable: false),
                    hero_class_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Heroes_HeroClasses_hero_class_id",
                        column: x => x.hero_class_id,
                        principalTable: "HeroClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Heroes_SubFactions_subfaction_id",
                        column: x => x.subfaction_id,
                        principalTable: "SubFactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubFactionAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subfaction_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFactionAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubFactionAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubFactionAndAbilityScopes_SubFactions_subfaction_id",
                        column: x => x.subfaction_id,
                        principalTable: "SubFactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitAndAbilitytScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitAndAbilitytScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_UnitAndAbilitytScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitAndAbilitytScopes_Units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "Units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtefactAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    artefact_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtefactAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_ArtefactAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtefactAndAbilityScopes_Artefacts_artefact_id",
                        column: x => x.artefact_id,
                        principalTable: "Artefacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtefactSetBonusAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    artefact_set_bonus_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtefactSetBonusAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_ArtefactSetBonusAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtefactSetBonusAndAbilityScopes_ArtefactSetBonuses_artefac~",
                        column: x => x.artefact_set_bonus_id,
                        principalTable: "ArtefactSetBonuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    skill_type = table.Column<int>(type: "integer", nullable: false),
                    competency_id = table.Column<int>(type: "integer", nullable: true),
                    subfaction_id = table.Column<int>(type: "integer", nullable: true),
                    ability_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.id);
                    table.ForeignKey(
                        name: "FK_Skills_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Skills_Competencies_competency_id",
                        column: x => x.competency_id,
                        principalTable: "Competencies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Skills_SubFactions_subfaction_id",
                        column: x => x.subfaction_id,
                        principalTable: "SubFactions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HeroAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hero_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_HeroAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroAndAbilityScopes_Heroes_hero_id",
                        column: x => x.hero_id,
                        principalTable: "Heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeroAndArtefactScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hero_id = table.Column<int>(type: "integer", nullable: false),
                    artefact_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroAndArtefactScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_HeroAndArtefactScopes_Artefacts_artefact_id",
                        column: x => x.artefact_id,
                        principalTable: "Artefacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroAndArtefactScopes_Heroes_hero_id",
                        column: x => x.hero_id,
                        principalTable: "Heroes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillDependencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    required_skill_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillDependencies", x => x.id);
                    table.ForeignKey(
                        name: "FK_SkillDependencies_Skills_required_skill_id",
                        column: x => x.required_skill_id,
                        principalTable: "Skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillDependencies_Skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "Skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    required_skill_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.id);
                    table.ForeignKey(
                        name: "FK_Spells_Skills_required_skill_id",
                        column: x => x.required_skill_id,
                        principalTable: "Skills",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SkillAndSpellScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    spell_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillAndSpellScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_SkillAndSpellScopes_Skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "Skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillAndSpellScopes_Spells_spell_id",
                        column: x => x.spell_id,
                        principalTable: "Spells",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpellAndAbilityScopes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    spell_id = table.Column<int>(type: "integer", nullable: false),
                    ability_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellAndAbilityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_SpellAndAbilityScopes_Abilities_ability_id",
                        column: x => x.ability_id,
                        principalTable: "Abilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpellAndAbilityScopes_Spells_spell_id",
                        column: x => x.spell_id,
                        principalTable: "Spells",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityAndEffectScopes_ability_id",
                table: "AbilityAndEffectScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_AbilityAndEffectScopes_effect_id",
                table: "AbilityAndEffectScopes",
                column: "effect_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtefactAndAbilityScopes_ability_id",
                table: "ArtefactAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtefactAndAbilityScopes_artefact_id",
                table: "ArtefactAndAbilityScopes",
                column: "artefact_id");

            migrationBuilder.CreateIndex(
                name: "IX_Artefacts_artefact_set_id",
                table: "Artefacts",
                column: "artefact_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_Artefacts_hero_class_id",
                table: "Artefacts",
                column: "hero_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtefactSetBonusAndAbilityScopes_ability_id",
                table: "ArtefactSetBonusAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtefactSetBonusAndAbilityScopes_artefact_set_bonus_id",
                table: "ArtefactSetBonusAndAbilityScopes",
                column: "artefact_set_bonus_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtefactSetBonuses_artefact_set_id",
                table: "ArtefactSetBonuses",
                column: "artefact_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtefactSetBonuses_hero_class_id",
                table: "ArtefactSetBonuses",
                column: "hero_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Competencies_subfaction_id",
                table: "Competencies",
                column: "subfaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_FactionAndAbilityScopes_ability_id",
                table: "FactionAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_FactionAndAbilityScopes_faction_id",
                table: "FactionAndAbilityScopes",
                column: "faction_id");

            migrationBuilder.CreateIndex(
                name: "IX_HeroAndAbilityScopes_ability_id",
                table: "HeroAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_HeroAndAbilityScopes_hero_id",
                table: "HeroAndAbilityScopes",
                column: "hero_id");

            migrationBuilder.CreateIndex(
                name: "IX_HeroAndArtefactScopes_artefact_id",
                table: "HeroAndArtefactScopes",
                column: "artefact_id");

            migrationBuilder.CreateIndex(
                name: "IX_HeroAndArtefactScopes_hero_id",
                table: "HeroAndArtefactScopes",
                column: "hero_id");

            migrationBuilder.CreateIndex(
                name: "IX_HeroClassAndAbilityScopes_ability_id",
                table: "HeroClassAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_HeroClassAndAbilityScopes_hero_class_id",
                table: "HeroClassAndAbilityScopes",
                column: "hero_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Heroes_hero_class_id",
                table: "Heroes",
                column: "hero_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Heroes_subfaction_id",
                table: "Heroes",
                column: "subfaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAndSpellScopes_skill_id",
                table: "SkillAndSpellScopes",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAndSpellScopes_spell_id",
                table: "SkillAndSpellScopes",
                column: "spell_id");

            migrationBuilder.CreateIndex(
                name: "IX_SkillDependencies_required_skill_id",
                table: "SkillDependencies",
                column: "required_skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_SkillDependencies_skill_id",
                table: "SkillDependencies",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ability_id",
                table: "Skills",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_competency_id",
                table: "Skills",
                column: "competency_id");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_subfaction_id",
                table: "Skills",
                column: "subfaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_SpellAndAbilityScopes_ability_id",
                table: "SpellAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_SpellAndAbilityScopes_spell_id",
                table: "SpellAndAbilityScopes",
                column: "spell_id");

            migrationBuilder.CreateIndex(
                name: "IX_Spells_required_skill_id",
                table: "Spells",
                column: "required_skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubFactionAndAbilityScopes_ability_id",
                table: "SubFactionAndAbilityScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubFactionAndAbilityScopes_subfaction_id",
                table: "SubFactionAndAbilityScopes",
                column: "subfaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_SubFactions_faction_id",
                table: "SubFactions",
                column: "faction_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAndAbilitytScopes_ability_id",
                table: "UnitAndAbilitytScopes",
                column: "ability_id");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAndAbilitytScopes_unit_id",
                table: "UnitAndAbilitytScopes",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Units_base_unit_id",
                table: "Units",
                column: "base_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Units_faction_id",
                table: "Units",
                column: "faction_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityAndEffectScopes");

            migrationBuilder.DropTable(
                name: "ArtefactAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "ArtefactSetBonusAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "FactionAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "HeroAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "HeroAndArtefactScopes");

            migrationBuilder.DropTable(
                name: "HeroClassAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "SkillAndSpellScopes");

            migrationBuilder.DropTable(
                name: "SkillDependencies");

            migrationBuilder.DropTable(
                name: "SpellAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "SubFactionAndAbilityScopes");

            migrationBuilder.DropTable(
                name: "UnitAndAbilitytScopes");

            migrationBuilder.DropTable(
                name: "Effects");

            migrationBuilder.DropTable(
                name: "ArtefactSetBonuses");

            migrationBuilder.DropTable(
                name: "Artefacts");

            migrationBuilder.DropTable(
                name: "Heroes");

            migrationBuilder.DropTable(
                name: "Spells");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "ArtefactSets");

            migrationBuilder.DropTable(
                name: "HeroClasses");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Competencies");

            migrationBuilder.DropTable(
                name: "SubFactions");

            migrationBuilder.DropTable(
                name: "Factions");
        }
    }
}
