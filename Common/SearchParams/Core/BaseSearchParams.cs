namespace Common.SearchParams.Core;

public class BaseSearchParams
{
    public string? SearchQuery { get; set; }
    public int StartIndex { get; set; }
    public int? ObjectsCount { get; set; }

    public BaseSearchParams(int startIndex = 0, int? objectsCount = null, string? searchQuery = null)
    {
        StartIndex = startIndex;
        ObjectsCount = objectsCount;
        SearchQuery = searchQuery;
    }
}