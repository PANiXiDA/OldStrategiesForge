namespace GamePlayService.Infrastructure.Enums;

public enum OutgoingMessageType
{
    ConnectionConfirmed = 0,
    DeploymentStart = 1,
    GameStart = 2,
    GameEnd = 3,
    GameClosed = 4,
    MessageAck = 5
}
