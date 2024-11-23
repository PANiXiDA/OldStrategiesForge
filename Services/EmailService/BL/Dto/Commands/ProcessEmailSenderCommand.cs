using MediatR;
using static EmailService.BL.Dto.Commands.ProcessEmailSenderCommand;

namespace EmailService.BL.Dto.Commands;

internal class ProcessEmailSenderCommand : IRequest<ProcessEmailResult>
{
    internal string Email { get; }
    internal EmailType Type { get; }
    internal int? PlayerId { get; }
    internal string? NewPassword { get; }

    public ProcessEmailSenderCommand(string email, EmailType type, int? playerId = null, string? newPassword = null)
    {
        Email = email;
        Type = type;
        PlayerId = playerId;
        NewPassword = newPassword;
    }

    public record ProcessEmailResult(bool IsSuccess);

    public enum EmailType
    {
        Confirmation,
        RecoveryPassword,
        ChangedPassword
    }
}
