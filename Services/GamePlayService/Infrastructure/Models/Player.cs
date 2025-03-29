using GameEngine.Domains;
using GameEngine.DTO.Enums;
using GamePlayService.Infrastructure.Enums;
using System.Net;

namespace GamePlayService.Infrastructure.Models;

public class Player
{
    public int Id { get; set; }
    public string SessionId { get; set; }
    public string BuildId { get; set; }
    public int CountMissedMoves { get; set; }
    public PlayerSide Side { get; set; }
    public Hero Hero { get; set; }
    public List<Unit> Units { get; set; }

    public List<IPEndPoint> IPEndPoints { get; set; } = new List<IPEndPoint>();

    public Player(
        int id,
        string sessionId,
        string buildId,
        IPEndPoint iPEndPoint,
        int countMissedMoves,
        PlayerSide side,
        Hero hero,
        List<Unit> units)
    {
        Id = id;
        SessionId = sessionId;
        BuildId = buildId;
        CountMissedMoves = countMissedMoves;
        Side = side;
        Hero = hero;
        Units = units;
        IPEndPoints.Add(iPEndPoint);
    }
}
