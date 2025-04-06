using GameEngine.Domains;
using GameEngine.DTO.ATBCalculator;
using GameEngine.DTO.PathFinderCalculator;
using GamePlayService.Infrastructure.Enums;

namespace GamePlayService.Infrastructure.Models;

public class RoundState
{
    public List<Tile> Grid { get; set; }
    public List<Hero> Heroes { get; set; }
    public List<Unit> Units { get; set; }
    public List<GameEntity> ATB { get; set; }
    public CommandType? LastCommand { get; set; }

    public RoundState(
        List<Tile> grid,
        List<Hero> heroes,
        List<Unit> units,
        List<GameEntity> atb,
        CommandType? lastCommand)
    {
        Grid = grid;
        Heroes = heroes;
        Units = units;
        ATB = atb;
        LastCommand = lastCommand;
    }
}
