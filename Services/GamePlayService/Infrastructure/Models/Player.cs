using GameEngine.Domains;
using System.Net;

namespace GamePlayService.Infrastructure.Models;

public class Player
{
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid BuildId { get; set; }
    public IPEndPoint IPEndPoint { get; set; }
    public int CountMissedMoves { get; set; }
    public Hero Hero { get; set; }
    public List<Unit> Units { get; set; }

    public Player(
        int id,
        Guid sessionId,
        Guid buildId,
        IPEndPoint iPEndPoint,
        int countMissedMoves,
        Hero hero,
        List<Unit> units)
    {
        Id = id;
        SessionId = sessionId;
        BuildId = buildId;
        IPEndPoint = iPEndPoint;
        CountMissedMoves = countMissedMoves;
        Hero = hero;
        Units = units;
    }
}
