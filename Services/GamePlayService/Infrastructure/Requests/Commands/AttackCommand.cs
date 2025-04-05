using GameEngine.DTO.PathFinderCalculator;

namespace GamePlayService.Infrastructure.Requests.Commands;

public class AttackCommand
{
    public Guid AttackerId { get; set; }
    public Guid DefenderId { get; set; }
    public bool MeleeAttack { get; set; }
    public Tile? AttackerTile { get; set; }

    public AttackCommand(
        Guid attackerId,
        Guid defenderId,
        bool meleeAttack,
        Tile? attackerTile)
    {
        AttackerId = attackerId;
        DefenderId = defenderId;
        MeleeAttack = meleeAttack;
        AttackerTile = attackerTile;
    }
}
