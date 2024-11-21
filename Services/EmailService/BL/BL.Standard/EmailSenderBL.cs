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
    private readonly SmtpConfiguration _smtpConfiguration;
    private readonly AesEncryption _encryption;
    private readonly string? _domen;

    public EmailSenderBL(IOptions<SmtpConfiguration> smtpConfiguration, AesEncryption encryption, IConfiguration configuration)
    {
        _smtpConfiguration = smtpConfiguration.Value;
        _encryption = encryption;
        _domen = configuration["Domen"];
    }

    public async Task SendConfirmationEmailAsync(string toEmail, int playerId)
    {
        var token = HttpUtility.UrlEncode(_encryption.Encrypt(playerId));
        var url = $"{_domen}/v1/auth/confirm?token={token}";

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
    }
}
