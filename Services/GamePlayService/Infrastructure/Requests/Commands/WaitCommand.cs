namespace GamePlayService.Infrastructure.Requests.Commands;

public class WaitCommand
{
    public Guid UnitId { get; set; }

    public WaitCommand(Guid unitId)
    {
        UnitId = unitId;
    }
}
