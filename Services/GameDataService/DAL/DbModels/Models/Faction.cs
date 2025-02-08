using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Faction
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<FactionAndAbilityScope> FactionAndAbilityScopes { get; set; } = new HashSet<FactionAndAbilityScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => FactionAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
