namespace GamePlayService.Infrastructure.Responses;

public class GameEndMessage
{
    public List<GameResult> GameResults { get; set; }

    public GameEndMessage(List<GameResult> gameResults)
    {
        GameResults = gameResults;
    }
}

public class GameResult
{
    public string NickName { get; set; }
    public int MmrChanges { get; set; }

    public GameResult(
        string nickName,
        int mmrChanges)
    {
        NickName = nickName;
        MmrChanges = mmrChanges;
    }
}