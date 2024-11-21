namespace EmailService.BL.BL.Interfaces;

internal interface IEmailSenderBL
{
    Task SendConfirmationEmailAsync(string toEmail, int playerId);
}
