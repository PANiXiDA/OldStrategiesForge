using Common.SearchParams.Core;

namespace Common.SearchParams.ProfileService;

public class FramesSearchParams : BaseSearchParams
{
    public bool? IsAvailable { get; set; }

    public FramesSearchParams() { }
    public FramesSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
