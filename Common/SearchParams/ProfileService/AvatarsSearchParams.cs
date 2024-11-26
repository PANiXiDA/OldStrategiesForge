using Common.SearchParams.Core;

namespace Common.SearchParams.ProfileService;

public class AvatarsSearchParams : BaseSearchParams
{
    public AvatarsSearchParams() { }
    public AvatarsSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
