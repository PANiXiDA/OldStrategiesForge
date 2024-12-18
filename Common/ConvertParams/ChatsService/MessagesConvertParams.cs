using Common.ConvertParams.Core;

namespace Common.ConvertParams.ChatsService;

public class MessagesConvertParams : BaseConvertParams
{
    public MessagesConvertParams() { }
    public MessagesConvertParams(
        bool isIncludeChildCategories = false,
        bool isIncludeParentCategories = false) : base(isIncludeChildCategories, isIncludeParentCategories)
    {
    }
}
