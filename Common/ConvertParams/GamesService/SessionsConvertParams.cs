using Common.ConvertParams.Core;

namespace Common.ConvertParams.GamesService;

public class SessionsConvertParams : BaseConvertParams
{
    public SessionsConvertParams() { }
    public SessionsConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
