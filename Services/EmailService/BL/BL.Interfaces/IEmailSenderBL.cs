namespace EmailService.BL.BL.Interfaces;

internal interface IEmailSenderBL
{
    Task<bool> SendConfirmationEmailAsync(string toEmail, int playerId);
    Task<bool> SendRecoveryPasswordAsync(string toEmail, int playerId);
    Task<bool> SendChangedPasswordAsync(string toEmail, string newPassword);
}
