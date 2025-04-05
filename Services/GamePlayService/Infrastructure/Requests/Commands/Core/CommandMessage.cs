using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Requests.Commands.Core;

public class CommandMessage
{
    public CommandType CommandType { get; set; }
    public string Command { get; set; }

    public CommandMessage(
        CommandType commandType,
        string command)
    {
        CommandType = commandType;
        Command = command;
    }
}
