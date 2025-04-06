using GameEngine.DTO.PathFinderCalculator;

namespace GamePlayService.Infrastructure.Requests.Commands;

public class MoveCommand
{
    public Guid UnitId { get; set; }
    public Tile Target { get; set; }

    public MoveCommand(
        Guid unitId,
        Tile target)
    {
        UnitId = unitId;
        Target = target;
    }
}
