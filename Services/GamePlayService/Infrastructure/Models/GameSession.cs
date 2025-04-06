using GameEngine.DTO.Enums;
using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Models;

public class GameSession
{
    public GameState GameState { get; set; }
    public GameType GameType { get; set; }
    public RoundState RoundState { get; set; }
    public List<RoundState> GameHistory { get; set; }
    public List<Player> Players { get; set; }

    public GameSession(
        GameState gameState,
        GameType gameType,
        RoundState roundState,
        List<RoundState> gameHistory,
        List<Player> players)
    {
        GameState = gameState;
        GameType = gameType;
        RoundState = roundState;
        GameHistory = gameHistory;
        Players = players;
    }
}
