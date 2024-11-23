using Common.Configurations;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Web;
using Tools.Encryption;
using EmailService.BL.BL.Interfaces;

namespace EmailService.BL.BL.Standard;

internal class EmailSenderBL : IEmailSenderBL
{
    private readonly ILogger<EmailSenderBL> _logger;
    private readonly SmtpConfiguration _smtpConfiguration;
    private readonly AesEncryption _encryption;
    private readonly string? _domen;

    public EmailSenderBL(
        ILogger<EmailSenderBL> logger,
        IOptions<SmtpConfiguration> smtpConfiguration,
        AesEncryption encryption,
        IConfiguration configuration)
    {
        _logger = logger;
        _smtpConfiguration = smtpConfiguration.Value;
        _encryption = encryption;
        _domen = configuration["Domen"];
    }

    public async Task<bool> SendConfirmationEmailAsync(string toEmail, int playerId)
    {
        try
        {
            var token = HttpUtility.UrlEncode(_encryption.Encrypt(playerId));
            var url = $"{_domen}/api/v1/auth/confirm?token={token}";

            string subject = "Confirmation";
            string body = $"To confirm your account, follow the link provided: <a href=\"{url}\" target=\"_blank\">Confirm</a>";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tactical Heroes Support", _smtpConfiguration.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.UseSsl);
                await client.AuthenticateAsync(_smtpConfiguration.SenderEmail, _smtpConfiguration.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send confirmation email to {toEmail}");

            return false;
        }
    }

    public async Task<bool> SendRecoveryPasswordAsync(string toEmail, int playerId)
    {
        try
        {
            var token = HttpUtility.UrlEncode(_encryption.Encrypt(playerId));
            var url = $"{_domen}/api/v1/auth/recovery?token={token}";

            string subject = "Recovery Password";
            string body = $"If you did not request password recovery, then please ignore this message.\n To restore your password, follow the link: <a href=\"{url}\" target=\"_blank\">Recovery Password</a>";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tactical Heroes Support", _smtpConfiguration.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.UseSsl);
                await client.AuthenticateAsync(_smtpConfiguration.SenderEmail, _smtpConfiguration.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send recovery password email to {toEmail}");

            return false;
        }
    }

    public async Task<bool> SendChangedPasswordAsync(string toEmail, string newPassword)
    {
        try
        {
            string subject = "Changed Password";
            string body = $"Your new password: {newPassword}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Tactical Heroes Support", _smtpConfiguration.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.UseSsl);
                await client.AuthenticateAsync(_smtpConfiguration.SenderEmail, _smtpConfiguration.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send change password email to {toEmail}");

            return false;
        }
    }
}
