using Common.SearchParams.Core;

namespace Common.SearchParams.GamesService;

public class GamesSearchParams : BaseSearchParams
{
    public GamesSearchParams() { }
    public GamesSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
