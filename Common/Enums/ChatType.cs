using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum ChatType
{
    [Display(Name = "Global chat")]
    Global = 0,

    [Display(Name = "Clan chat")]
    Clan = 1,

    [Display(Name = "Private chat")]
    Private = 2
}
