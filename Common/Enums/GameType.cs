using System.ComponentModel.DataAnnotations;

namespace Common.Enums;

public enum GameType
{
    [Display(Name = "Дуэль")]
    Duel = 0,

    [Display(Name = "Командный бой 2х2")]
    Team2х2 = 1,
}
