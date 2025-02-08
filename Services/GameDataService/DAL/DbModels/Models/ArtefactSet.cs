using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class ArtefactSet
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<ArtefactSetBonus> ArtefactSetBonuses { get; set; } = new HashSet<ArtefactSetBonus>();
    public virtual ICollection<Artefact> Artefacts { get; set; } = new HashSet<Artefact>();
}
