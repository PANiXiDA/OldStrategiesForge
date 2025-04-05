using GamePlayService.Infrastructure.Requests;
using System.Net;

namespace GamePlayService.Handlers.Interfaces.Core;

public interface IBaseHandler
{
    Task Handle(IncomingMessage message, IPEndPoint clientEndpoint);
}
