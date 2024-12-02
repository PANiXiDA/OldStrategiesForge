using Common.Configurations;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Tools.Encryption;
using EmailService.BL.BL.Interfaces;
using Tools.Redis;
using System.Text;

internal class EmailSenderBL : IEmailSenderBL
{
    private readonly ILogger<EmailSenderBL> _logger;
    private readonly SmtpConfiguration _smtpConfiguration;
    private readonly AesEncryption _encryption;
    private readonly IRedisCache _redisCache;

    private readonly string? _domen;

    private const int expiredAt = 15;

    public EmailSenderBL(
        ILogger<EmailSenderBL> logger,
        IOptions<SmtpConfiguration> smtpConfiguration,
        AesEncryption encryption,
        IConfiguration configuration,
        IRedisCache redisCache)
    {
        _logger = logger;
        _smtpConfiguration = smtpConfiguration.Value;
        _encryption = encryption;
        _redisCache = redisCache;

        _domen = configuration["Domen"];
    }

    private string GetEmailTemplate()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "BL", "Extensions", "TemplatesForEmails", "email_template.html");
        return File.ReadAllText(path);
    }

    private string PopulateTemplate(string template, string subject, string content)
    {
        return template.Replace("{subject}", subject)
                       .Replace("{content}", content);
    }

    private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
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
            _logger.LogError(ex, $"Failed to send email to {toEmail}");
            return false;
        }
    }

    public async Task<bool> SendConfirmationEmailAsync(string toEmail, int playerId)
    {
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(_encryption.Encrypt(playerId)));
        var url = $"{_domen}/api/v1/auth/confirm?token={token}";

        await _redisCache.SetAsync($"confirm:{token}", true, TimeSpan.FromMinutes(expiredAt));

        string subject = "Confirmation";
        string content = $"To confirm your account, follow the link provided: <a href=\"{url}\" target=\"_blank\">Confirm</a>";

        var template = GetEmailTemplate();
        var body = PopulateTemplate(template, subject, content);

        return await SendEmailAsync(toEmail, subject, body);
    }

    public async Task<bool> SendRecoveryPasswordAsync(string toEmail, int playerId)
    {
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(_encryption.Encrypt(playerId)));

        var url = $"{_domen}/api/v1/auth/recovery?token={token}";

        await _redisCache.SetAsync($"recovery:{token}", true, TimeSpan.FromMinutes(expiredAt));

        string subject = "Recovery Password";
        string content = $"If you did not request password recovery, please ignore this message.<br>To restore your password, follow the link: <a href=\"{url}\" target=\"_blank\">Recovery Password</a>";

        var template = GetEmailTemplate();
        var body = PopulateTemplate(template, subject, content);

        return await SendEmailAsync(toEmail, subject, body);
    }

    public async Task<bool> SendChangedPasswordAsync(string toEmail, string newPassword)
    {
        string subject = "Changed Password";
        string content = $"Your new password: <strong>{newPassword}</strong>";

        var template = GetEmailTemplate();
        var body = PopulateTemplate(template, subject, content);

        return await SendEmailAsync(toEmail, subject, body);
    }
}
