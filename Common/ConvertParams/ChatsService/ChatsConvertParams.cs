using Common.ConvertParams.Core;

namespace Common.ConvertParams.ChatsService;
public class ChatsConvertParams : BaseConvertParams
{
    public ChatsConvertParams() { }
    public ChatsConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
