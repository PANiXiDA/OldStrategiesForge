using Common.SearchParams.Core;

namespace Common.SearchParams.ChatsService;

public class MessagesSearchParams : BaseSearchParams
{
    public Guid? ChatId { get; set; }
    public bool? GetHistoryChat { get; set; }

    public MessagesSearchParams() { }
    public MessagesSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
