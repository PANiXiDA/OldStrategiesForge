using Common.SearchParams.Core;

namespace Common.SearchParams.ProfileService;
public class TokensSearchParams : BaseSearchParams
{
    public string? RefreshToken { get; set; }
    public int? PlayerId { get; set; }

    public TokensSearchParams() { }
    public TokensSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
    {
    }
}
