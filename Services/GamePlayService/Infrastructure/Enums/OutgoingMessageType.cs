namespace GamePlayService.Infrastructure.Enums;

public enum OutgoingMessageType
{
    ConnectionConfirmed = 0,
    DeploymentStart = 1,
    GameStart = 2,
    Command = 3,
    GameEnd = 4,
    GameClosed = 5,
    MessageAck = 6
}
