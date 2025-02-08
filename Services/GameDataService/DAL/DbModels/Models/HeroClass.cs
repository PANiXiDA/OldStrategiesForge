using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class HeroClass
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<HeroClassAndAbilityScope> HeroClassAndAbilityScopes { get; set; } = new HashSet<HeroClassAndAbilityScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => HeroClassAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
