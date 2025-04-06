using GameEngine.Domains.Enums;

namespace GamePlayService.Infrastructure.Requests.Commands;

public class UseAbilityCommand
{
    public Guid UnitId { get; set; }
    public AbilityType AbilityType { get; set; }

    public UseAbilityCommand(Guid unitId, AbilityType abilityType)
    {
        UnitId = unitId;
        AbilityType = abilityType;
    }
}
