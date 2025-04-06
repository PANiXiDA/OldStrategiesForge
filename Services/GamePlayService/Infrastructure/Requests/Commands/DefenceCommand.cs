namespace GamePlayService.Infrastructure.Requests.Commands;

public class DefenceCommand
{
    public Guid UnitId { get; set; }

    public DefenceCommand(Guid unitId)
    {
        UnitId = unitId;
    }
}
