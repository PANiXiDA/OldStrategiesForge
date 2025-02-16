using GameDataService.DAL.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDataService.DAL.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    public virtual DbSet<Faction> Factions { get; set; }
    public virtual DbSet<FactionAndAbilityScope> FactionAndAbilityScopes { get; set; }
    public virtual DbSet<Subfaction> Subfactions { get; set; }
    public virtual DbSet<SubfactionAndAbilityScope> SubfactionAndAbilityScopes { get; set; }
    public virtual DbSet<Unit> Units { get; set; }
    public virtual DbSet<UnitAndAbilitytScope> UnitAndAbilitytScopes { get; set; }
    public virtual DbSet<Hero> Heroes { get; set; }
    public virtual DbSet<HeroAndArtefactScope> HeroAndArtefactScopes { get; set; }
    public virtual DbSet<HeroAndAbilityScope> HeroAndAbilityScopes { get; set; }
    public virtual DbSet<HeroClass> HeroClasses { get; set; }
    public virtual DbSet<HeroClassAndAbilityScope> HeroClassAndAbilityScopes { get; set; }
    public virtual DbSet<Artefact> Artefacts { get; set; }
    public virtual DbSet<ArtefactAndAbilityScope> ArtefactAndAbilityScopes { get; set; }
    public virtual DbSet<ArtefactSet> ArtefactSets { get; set; }
    public virtual DbSet<ArtefactSetBonus> ArtefactSetBonuses { get; set; }
    public virtual DbSet<ArtefactSetBonusAndAbilityScope> ArtefactSetBonusAndAbilityScopes { get; set; }
    public virtual DbSet<Competence> Competencies { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<SkillDependency> SkillDependencies { get; set; }
    public virtual DbSet<SkillAndSpellScope> SkillAndSpellScopes { get; set; }
    public virtual DbSet<Spell> Spells { get; set; }
    public virtual DbSet<SpellAndAbilityScope> SpellAndAbilityScopes { get; set; }
    public virtual DbSet<Ability> Abilities { get; set; }
    public virtual DbSet<AbilityAndEffectScope> AbilityAndEffectScopes { get; set; }
    public virtual DbSet<Effect> Effects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillDependency>()
            .HasOne(sd => sd.Skill)
            .WithMany(s => s.RequiredSkillDependencies)
            .HasForeignKey(sd => sd.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SkillDependency>()
            .HasOne(sd => sd.RequiredSkill)
            .WithMany(s => s.DependentSkillDependencies)
            .HasForeignKey(sd => sd.RequiredSkillId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}