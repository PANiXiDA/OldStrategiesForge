using Common.ConvertParams.Core;

namespace Common.ConvertParams.ProfileService;
public class TokensConvertParams : BaseConvertParams
{
    public TokensConvertParams() { }
    public TokensConvertParams(
    bool isIncludeChildCategories = false,
    bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
