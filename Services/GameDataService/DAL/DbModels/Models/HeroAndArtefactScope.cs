using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class HeroAndArtefactScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("hero_id")]
    public int HeroId { get; set; }

    [Column("artefact_id")]
    public int ArtefactId { get; set; }

    public virtual Hero? Hero { get; set; }
    public virtual Artefact? Artefact { get; set; }
}
