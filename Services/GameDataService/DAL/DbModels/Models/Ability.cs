using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Ability
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<AbilityAndEffectScope> AbilityAndEffectScopes { get; set; } = new HashSet<AbilityAndEffectScope>();

    [NotMapped]
    public IEnumerable<Effect> Effects => AbilityAndEffectScopes
        .Where(item => item.Effect != null)
        .Select(item => item.Effect!);
}
