using Common.ConvertParams.Core;

namespace Common.ConvertParams.ProfileService;
public class PlayersConvertParams : BaseConvertParams
{
    public PlayersConvertParams() { }
    public PlayersConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
