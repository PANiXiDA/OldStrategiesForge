using Common.ConvertParams.Core;

namespace Common.ConvertParams.PlayersService;
public class PlayersConvertParams : BaseConvertParams
{
    public PlayersConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
