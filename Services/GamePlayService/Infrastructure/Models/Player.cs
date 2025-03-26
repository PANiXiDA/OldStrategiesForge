using GameEngine.Domains;
using System.Net;

namespace GamePlayService.Infrastructure.Models;

public class Player
{
    public int Id { get; set; }
    public string SessionId { get; set; }
    public string BuildId { get; set; }
    public int CountMissedMoves { get; set; }
    public Hero Hero { get; set; }
    public List<Unit> Units { get; set; }

    public List<IPEndPoint> IPEndPoints { get; set; } = new List<IPEndPoint>();

    public Player(
        int id,
        string sessionId,
        string buildId,
        IPEndPoint iPEndPoint,
        int countMissedMoves,
        Hero hero,
        List<Unit> units)
    {
        Id = id;
        SessionId = sessionId;
        BuildId = buildId;
        CountMissedMoves = countMissedMoves;
        Hero = hero;
        Units = units;
        IPEndPoints.Add(iPEndPoint);
    }
}
