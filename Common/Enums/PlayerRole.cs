using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum PlayerRole
{
    [Display(Name = "Разработчик")]
    Developer = 0,

    [Display(Name = "Игрок")]
    Player = 1,

    [Display(Name = "Администратор")]
    Admin = 2,

    [Display(Name = "Модератор")]
    Moderator = 3
}
