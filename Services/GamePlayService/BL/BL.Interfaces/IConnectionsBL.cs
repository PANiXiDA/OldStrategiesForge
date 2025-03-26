using GamePlayService.Infrastructure.Models;
using Games.Entities.Gen;
using System.Net;

namespace GamePlayService.BL.BL.Interfaces;

public interface IConnectionsBL
{
    Task HandleConnection(GameSession gameSession, Session session, IPEndPoint clientEndpoint);
    Task<Session?> GetUserSession(string authToken, string sessionId);
}
