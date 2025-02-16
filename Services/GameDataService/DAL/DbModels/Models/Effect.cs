using Common.Enums;
using GameData.Enums.Gen;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Effect
{
    [Column("id")]
    public int Id { get; set; }

    [Column("effect_type")]
    public EffectType EffectType { get; set; }

    [Column("value")]
    public double? Value { get; set; }

    [Column("duration")]
    public double? Duration { get; set; }

    [Column("parametrs")]
    public string Parameters { get; set; } = "{}";
}
