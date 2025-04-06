namespace GamePlayService.Infrastructure.Requests.Commands;

public class UseMagicCommand
{
    public Guid UnitId { get; set; }

    public UseMagicCommand(Guid unitId)
    {
        UnitId = unitId;
    }
}
