using Common;
using Microsoft.AspNetCore.Mvc;
using Tools.RabbitMQ;

namespace FrontendService.Controllers;

public class EmailController : Controller
{
    private readonly ILogger<EmailController> _logger;
    private readonly IRabbitMQClient _rabbitMQClient;

    public EmailController(ILogger<EmailController> logger, IRabbitMQClient rabbitMQClient)
    {
        _logger = logger;
        _rabbitMQClient = rabbitMQClient;
    }

    [HttpPost]
    public JsonResult SubscribeToNotifications(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Пустой email адрес.");
                return Json(new { ok = false, message = "Invalid email" });
            }

            _logger.LogInformation($"Отправка запроса на подписку для {email}.");
            _rabbitMQClient.SendMessage(email, Constants.RabbitMqQueues.SubscribeToNotifications);
            _logger.LogInformation($"Запрос на подписку для {email} успешно отправлен.");

            return Json(new { ok = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при отправке запроса на подписку для {email}");

            return Json(new { ok = false, message = "Ошибка при подписке на уведомления" });
        }
    }
}
