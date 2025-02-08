using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class ArtefactAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("artefact_id")]
    public int ArtefactId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual Artefact? Artefact { get; set; }
    public virtual Ability? Ability { get; set; }
}
