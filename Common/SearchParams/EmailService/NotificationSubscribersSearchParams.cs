using Common.SearchParams.Core;

namespace Common.SearchParams.EmailService;

public class NotificationSubscribersSearchParams : BaseSearchParams
{
    public string? Email { get; set; }

    public NotificationSubscribersSearchParams() { }
    public NotificationSubscribersSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
