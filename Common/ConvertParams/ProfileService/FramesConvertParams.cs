using Common.ConvertParams.Core;

namespace Common.ConvertParams.ProfileService;

public class FramesConvertParams : BaseConvertParams
{
    public FramesConvertParams() { }
    public FramesConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
