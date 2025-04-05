using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests.Commands;

namespace GamePlayService.BL.BL.Interfaces;

public interface ICommandsBL
{
    void Move(MoveCommand command, GameSession gameSession, int playerId);
    void Attack(AttackCommand command, GameSession gameSession, int playerId);
    void Defence(DefenceCommand command, GameSession gameSession, int playerId);
    void Wait(WaitCommand command, GameSession gameSession, int playerId);
}
