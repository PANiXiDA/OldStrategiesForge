using Common.SearchParams.Core;

namespace Common.SearchParams.PlayersService;
public class PlayersSearchParams : BaseSearchParams
{
    public string? Email { get; set; }
    public string? Nickname { get; set; }
    public PlayersSearchParams() { }
    public PlayersSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
