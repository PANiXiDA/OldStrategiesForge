using GameData.Entities.Gen;
using GameData.Enums.Gen;
using System.ComponentModel.DataAnnotations;

namespace APIGateway.Infrastructure.GameDataService.Models.Effects;

public class EffectDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "EffectType is required.")]
    public EffectType EffectType { get; set; }

    public double? Value { get; set; }
    public double? Duration { get; set; }

    public string Parameters { get; set; } = "{}";

    public static EffectDto FromEntity(Effect obj)
    {
        return new EffectDto
        {
            Id = obj.Id,
            EffectType = obj.EffectType,
            Value = obj.Value,
            Duration = obj.Duration,
            Parameters = obj.Parameters
        };
    }

    public static Effect ToEntity(EffectDto obj)
    {
        var entity = new Effect
        {
            Id = obj.Id ?? 0,
            EffectType = obj.EffectType,
            Value = obj.Value,
            Duration = obj.Duration,
            Parameters = obj.Parameters
        };

        return entity;
    }

    public static List<EffectDto> FromEntitiesList(IEnumerable<Effect> list)
    {
        return list?.Select(FromEntity).Where(item => item != null).ToList() ?? new List<EffectDto>();
    }

    public static List<Effect> ToEntitiesList(IEnumerable<EffectDto> list)
    {
        return list?.Select(ToEntity).Where(item => item != null).ToList() ?? new List<Effect>();
    }
}
