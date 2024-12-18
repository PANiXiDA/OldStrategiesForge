using Common.SearchParams.Core;

namespace Common.SearchParams.ChatsService;

public class ChatsSearchParams : BaseSearchParams
{
    public ChatsSearchParams() { }
    public ChatsSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
