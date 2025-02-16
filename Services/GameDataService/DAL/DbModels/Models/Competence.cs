using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Competence
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("subfaction_id")]
    public int SubfactionId { get; set; }

    public virtual Subfaction? Subfaction { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
}
