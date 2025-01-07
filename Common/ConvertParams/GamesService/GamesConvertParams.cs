using Common.ConvertParams.Core;

namespace Common.ConvertParams.GamesService;

public class GamesConvertParams : BaseConvertParams
{
    public GamesConvertParams() { }
    public GamesConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
