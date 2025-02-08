using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Unit
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("attack")]
    public int Attack { get; set; }

    [Column("defence")]
    public int Defence { get; set; }

    [Column("health")]
    public int Health { get; set; }

    [Column("min_damage")]
    public int MinDamage { get; set; }

    [Column("max_damage")]
    public int MaxDamage { get; set; }

    [Column("initiative")]
    public double Initiative { get; set; }

    [Column("speed")]
    public int Speed { get; set; }

    [Column("range")]
    public int? Range { get; set; }

    [Column("arrows")]
    public int? Arrows { get; set; }

    [Column("morale")]
    public int Morale { get; set; }

    [Column("luck")]
    public int Luck { get; set; }

    [Column("faction_id")]
    public int? FactionId { get; set; }

    [Column("base_unit_id")]
    public int? BaseUnitId { get; set; }

    public virtual Faction? Faction { get; set; }
    public virtual Unit? BaseUnit { get; set; }
    public virtual ICollection<Unit> Upgrades { get; set; } = new HashSet<Unit>();
    public virtual ICollection<UnitAndAbilitytScope> UnitAndAbilitytScopes { get; set; } = new HashSet<UnitAndAbilitytScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => UnitAndAbilitytScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
