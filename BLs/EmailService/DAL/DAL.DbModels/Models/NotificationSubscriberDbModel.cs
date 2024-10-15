using BaseDAL.Models;

namespace EmailService.DAL.DAL.DbModels.Models;

public class NotificationSubscriberDbModel : BasePostgresDbModel
{
    public string Email { get; set; } = string.Empty;
}
