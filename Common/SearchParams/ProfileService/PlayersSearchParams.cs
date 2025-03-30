using Common.SearchParams.Core;

namespace Common.SearchParams.ProfileService;
public class PlayersSearchParams : BaseSearchParams
{
    public string? Email { get; set; }
    public string? Nickname { get; set; }
    public bool? IsRegistrationCheck { get; set; }
    public List<int> Ids { get; set; } = new List<int>();

    public PlayersSearchParams() { }
    public PlayersSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
