using EmailService.BL.BL.Interfaces;
using EmailService.BL.Dto.Commands;
using MediatR;
using static EmailService.BL.Dto.Commands.ProcessEmailSenderCommand;

namespace EmailService.BL.Dto.Handlers;

internal class ProcessEmailSenderHandler : IRequestHandler<ProcessEmailSenderCommand, ProcessEmailResult>
{
    private readonly IEmailSenderBL _emailSenderBL;

    public ProcessEmailSenderHandler(IEmailSenderBL emailSenderBL)
    {
        _emailSenderBL = emailSenderBL;
    }

    public async Task<ProcessEmailResult> Handle(ProcessEmailSenderCommand request, CancellationToken cancellationToken)
    {
        bool isSuccess = false;
        if (request.Type == EmailType.Confirmation && request.PlayerId.HasValue)
        {
            isSuccess = await _emailSenderBL.SendConfirmationEmailAsync(request.Email, request.PlayerId.Value);
        }
        else if (request.Type == EmailType.RecoveryPassword && request.PlayerId.HasValue)
        {
            isSuccess = await _emailSenderBL.SendRecoveryPasswordAsync(request.Email, request.PlayerId.Value);
        }
        else if (request.Type == EmailType.ChangedPassword && !string.IsNullOrEmpty(request.NewPassword))
        {
            isSuccess = await _emailSenderBL.SendChangedPasswordAsync(request.Email, request.NewPassword);
        }

        return new ProcessEmailResult(isSuccess);
    }

}
