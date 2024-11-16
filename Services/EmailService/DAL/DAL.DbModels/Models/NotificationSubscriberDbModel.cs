using BaseDAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmailService.DAL.DAL.DbModels.Models;

internal class NotificationSubscriberDbModel : BasePostgresDbModel
{
    [Column("email")]
    public string Email { get; set; } = string.Empty;
}
