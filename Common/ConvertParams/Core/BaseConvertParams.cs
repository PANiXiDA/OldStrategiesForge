namespace Common.ConvertParams.Core;
public class BaseConvertParams
{
    public bool IsIncludeChildCategories { get; set; }
    public bool IsIncludeParentCategories { get; set; }

    public BaseConvertParams(bool isIncludeChildCategories = false, bool isIncludeParentCategories = false)
    {
        IsIncludeChildCategories = isIncludeChildCategories;
        IsIncludeParentCategories = isIncludeParentCategories;
    }
}
