using Common.ConvertParams.Core;

namespace Common.ConvertParams.ProfileService;

public class AvatarsConvertParams : BaseConvertParams
{
    public AvatarsConvertParams() { }
    public AvatarsConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
