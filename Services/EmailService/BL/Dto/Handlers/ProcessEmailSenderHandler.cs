using EmailService.BL.BL.Interfaces;
using EmailService.BL.Dto.Commands;
using MediatR;

namespace EmailService.BL.Dto.Handlers;

internal class ProcessEmailSenderHandler : IRequestHandler<ProcessEmailSenderCommand, Unit>
{
    private readonly IEmailSenderBL _emailSenderBL;

    public ProcessEmailSenderHandler(IEmailSenderBL emailSenderBL)
    {
        _emailSenderBL = emailSenderBL;
    }

    public async Task<Unit> Handle(ProcessEmailSenderCommand request, CancellationToken cancellationToken)
    {
        await _emailSenderBL.SendConfirmationEmailAsync(request.Email, request.PlayerId);

        return Unit.Value;
    }
}
