using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Competency
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("subfaction_id")]
    public int SubFactionId { get; set; }

    public virtual SubFaction? SubFaction { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
}
