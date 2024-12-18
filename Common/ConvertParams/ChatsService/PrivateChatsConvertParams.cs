using Common.ConvertParams.Core;

namespace Common.ConvertParams.ChatsService;

public class PrivateChatsConvertParams : BaseConvertParams
{
    public PrivateChatsConvertParams() { }
    public PrivateChatsConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
