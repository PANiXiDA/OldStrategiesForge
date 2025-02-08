using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class SubFaction
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("faction_id")]
    public int FactionId { get; set; }

    public virtual Faction? Faction { get; set; }

    public virtual ICollection<SubFactionAndAbilityScope> SubFactionAndAbilityScopes { get; set; } = new HashSet<SubFactionAndAbilityScope>();
    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => SubFactionAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
