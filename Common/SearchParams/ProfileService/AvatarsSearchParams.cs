using Common.SearchParams.Core;

namespace Common.SearchParams.ProfileService;

public class AvatarsSearchParams : BaseSearchParams
{
    public bool? IsAvailable { get; set; }

    public AvatarsSearchParams() { }
    public AvatarsSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
