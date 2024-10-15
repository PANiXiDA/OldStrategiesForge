using BaseBL.Models;

namespace EmailService.BL.BL.Models;

internal class NotificationSubscriberEntity : BasePostgresEntity
{
    internal string Email { get; set; }

    public NotificationSubscriberEntity(string email) : base()
    {
        Email = email;
    }

    public NotificationSubscriberEntity(int id, DateTime createdAt, DateTime updateAt, DateTime? deletedAt, string email)
        : base(id, createdAt, updateAt, deletedAt)
    {
        Email = email;
    }
}
