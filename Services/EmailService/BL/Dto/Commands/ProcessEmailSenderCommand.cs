using MediatR;

namespace EmailService.BL.Dto.Commands;

internal class ProcessEmailSenderCommand : IRequest<Unit>
{
    internal string Email { get; }
    internal int PlayerId { get; }

    public ProcessEmailSenderCommand(string email, int playerId)
    {
        Email = email;
        PlayerId = playerId;
    }
}
