using Common.SearchParams.Core;

namespace Common.SearchParams.ChatsService;

public class PrivateChatsSearchParams : BaseSearchParams
{
    public PrivateChatsSearchParams() { }
    public PrivateChatsSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
