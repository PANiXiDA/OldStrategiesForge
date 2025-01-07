using Profile.Players.Gen;

namespace GamesService.Dto.Entities;

public class PlayerQueue
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public int Mmr { get; set; }
    public int Rank { get; set; }
    public int Level { get; set; }
    public Avatar Avatar { get; set; }
    public Frame Frame { get; set; }

    public int SearchWaitingSeconds { get; set; }
    public int SearchMmrRange { get; set; }

    public PlayerQueue(
        int id,
        string nickname,
        int mmr,
        int rank,
        int level,
        Avatar avatar,
        Frame frame,
        int searchWaitingSeconds = 0,
        int searchMmrRange = 50)
    {
        Id = id;
        Nickname = nickname;
        Mmr = mmr;
        Rank = rank;
        Level = level;
        Avatar = avatar;
        Frame = frame;
        SearchWaitingSeconds = searchWaitingSeconds;
        SearchMmrRange = searchMmrRange;
    }

    public void UpdateSearchSettings()
    {
        SearchWaitingSeconds += 10;
        SearchMmrRange += 10;
    }
}