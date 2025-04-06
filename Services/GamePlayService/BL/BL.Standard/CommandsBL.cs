using GameEngine.Domains;
using GameEngine.Domains.Core;
using GameEngine.Domains.Enums;
using GameEngine.DTO.ATBCalculator;
using GameEngine.DTO.DamageCalculator;
using GameEngine.DTO.PathFinderCalculator;
using GameEngine.Interfaces;
using GamePlayService.BL.BL.Interfaces;
using GamePlayService.Infrastructure.Enums;
using GamePlayService.Infrastructure.Models;
using GamePlayService.Infrastructure.Requests.Commands;

namespace GamePlayService.BL.BL.Standard;

public class CommandsBL : ICommandsBL
{
    private readonly ILogger<CommandsBL> _logger;

    private readonly IATBCalculator _atbCalculator;
    private readonly IPathFinderCalculator _pathFinderCalculator;
    private readonly IDamageCalculator _damageCalculator;

    public CommandsBL(
        ILogger<CommandsBL> logger,
        IATBCalculator atbCalculator,
        IPathFinderCalculator pathFinderCalculator,
        IDamageCalculator damageCalculator)
    {
        _logger = logger;

        _atbCalculator = atbCalculator;
        _pathFinderCalculator = pathFinderCalculator;
        _damageCalculator = damageCalculator;
    }

    public void Move(MoveCommand command, GameSession gameSession, int playerId)
    {
        if (GetPlayerGameObject(gameSession, playerId, command.UnitId) is not Unit unit)
        {
            return;
        }

        var (currentTile, targetTile) = ValidateMovement(gameSession, unit, command.Target);
        if (currentTile == null || targetTile == null)
        {
            return;
        }

        var atb = ValidateTurn(gameSession, unit.Id);
        if (atb == null)
        {
            return;
        }

        gameSession.GameHistory.Add(gameSession.RoundState);

        ApplyTurn(gameSession, atb, CommandType.Move);
        UpdateTileOccupation(currentTile, targetTile, unit.Id);
    }

    public void Attack(AttackCommand command, GameSession gameSession, int playerId)
    {
        var attacker = GetPlayerGameObject(gameSession, playerId, command.AttackerId);
        if (attacker == null)
        {
            return;
        }

        var defender = (gameSession.RoundState.Units.FirstOrDefault(unit => unit.Id == command.DefenderId));
        if (defender == null)
        {
            _logger.LogError($"Юнит с id: {command.DefenderId} не принадлежит игровой сессии: {gameSession}.");
            return;
        }

        (Tile? currentTile, Tile? targetTile) = (null, null);
        if (attacker is Unit unitAttacker)
        {
            if (command.AttackerTile == null)
            {
                _logger.LogError($"Атакующий с id: {command.AttackerId} оказался юнитом, но таргет клетка не указана.");
                return;
            }

            (currentTile, targetTile) = ValidateMovement(gameSession, unitAttacker, command.AttackerTile);
            if (currentTile == null || targetTile == null)
            {
                return;
            }
        }

        var atb = ValidateTurn(gameSession, attacker.Id);
        if (atb == null)
        {
            return;
        }

        gameSession.GameHistory.Add(gameSession.RoundState);

        if (attacker is Unit)
        {
            UpdateTileOccupation(currentTile!, targetTile!, attacker.Id);
        }

        ApplyTurn(gameSession, atb, CommandType.Attack);
        ApplyAttack(attacker, defender);
        RemoveDeathUnits(gameSession);
    }

    public void Defence(DefenceCommand command, GameSession gameSession, int playerId)
    {
        if (GetPlayerGameObject(gameSession, playerId, command.UnitId) is not Unit unit)
        {
            return;
        }

        var atb = ValidateTurn(gameSession, unit.Id);
        if (atb == null)
        {
            return;
        }

        gameSession.GameHistory.Add(gameSession.RoundState);

        unit.Effects.Add(new Effect(
            effectType: EffectType.Defence,
            value: unit.Defence * 0.3,
            duration: 0,
            parameters: string.Empty));

        ApplyTurn(gameSession, atb, CommandType.Defence);
    }

    public void Wait(WaitCommand command, GameSession gameSession, int playerId)
    {
        if (GetPlayerGameObject(gameSession, playerId, command.UnitId) is not Unit unit)
        {
            return;
        }

        var atb = ValidateTurn(gameSession, unit.Id);
        if (atb == null)
        {
            return;
        }

        atb = _atbCalculator.ShiftATBPosition(new ATBPositionShiftContext()
        {
            CurrentATBState = gameSession.RoundState.ATB,
            UnitId = unit.Id,
            ShiftFactor = 0.5
        });

        gameSession.GameHistory.Add(gameSession.RoundState);

        ApplyTurn(gameSession, atb, CommandType.Wait);
    }

    private GameObject? GetPlayerGameObject(GameSession gameSession, int playerId, Guid gameObjectId)
    {
        var player = gameSession.Players.FirstOrDefault(player => player.Id == playerId);
        if (player == null)
        {
            _logger.LogError($"Игрок с id: {playerId} не участвует в игровой сессии: {gameSession}.");
            return null;
        }

        var gameObject = (player.Units.FirstOrDefault(unit => unit.Id == gameObjectId) as GameObject)
            ?? (player.Hero.Id == gameObjectId ? (GameObject)player.Hero : null);
        if (gameObject == null)
        {
            _logger.LogError($"Юнит или герой с id: {gameObjectId} не принадлежит игроку: {playerId}.");
            return null;
        }

        return gameObject;
    }

    private (Tile? currentTile, Tile? targetTile) ValidateMovement(GameSession gameSession, Unit unit, Tile target)
    {
        var currentTile = gameSession.RoundState.Grid.FirstOrDefault(tile => tile.OccupiedUnitId.HasValue && tile.OccupiedUnitId == unit.Id);
        if (currentTile == null)
        {
            _logger.LogError($"У юнита с id: {unit.Id} нет клетки.");
            return (null, null);
        }

        var targetTile = gameSession.RoundState.Grid.FirstOrDefault(tile => tile.X == target.X && tile.Y == target.Y);
        if (targetTile == null)
        {
            _logger.LogError($"Клетки с координами X: {target.X} Y: {target.Y} не существует в текущей игровой сессии.");
            return (null, null);
        }

        var isReachableTile = _pathFinderCalculator.IsReachable(new PathfindingContext(
            moveRange: unit.Speed,
            ignoringObstacles: unit.Abilities.Any(ability => ability.AbilityType == AbilityType.Fly), // TODO: переписать лучше, когда появятся новые абилки
            grid: gameSession.RoundState.Grid,
            start: currentTile,
            target: targetTile));
        if (!isReachableTile)
        {
            _logger.LogError($"Клетка: {targetTile} недостижима юнитом с id: {unit.Id} из клетки {currentTile}");
            return (null, null);
        }

        return (currentTile, targetTile);
    }

    private void UpdateTileOccupation(Tile currentTile, Tile targetTile, Guid unitId)
    {
        currentTile.OccupiedUnitId = null;
        currentTile.IsWalkable = true;

        targetTile.OccupiedUnitId = unitId;
        targetTile.IsWalkable = false;
    }

    private List<GameEntity>? ValidateTurn(GameSession gameSession, Guid gameObjectId)
    {
        var unitsInitiatives = gameSession.RoundState.Units.Select(unit => new GameEntityInitiative(unit.Id, unit.CurrentInitiative)).ToList();
        var heroesInitiatives = gameSession.RoundState.Heroes.Select(hero => new GameEntityInitiative(hero.Id, hero.Initiative)).ToList();

        var nextTurn = _atbCalculator.GetNextTurn(new ATBCalculationContext()
        {
            CurrentATBState = gameSession.RoundState.ATB,
            GameEntitiesInitiatives = unitsInitiatives.Concat(heroesInitiatives).ToList()
        });
        if (nextTurn.NextGameObjectId != gameObjectId)
        {
            _logger.LogError($"Сейчас ход юнита с id: {nextTurn.NextGameObjectId}, а не {gameObjectId}.");
            return null;
        }

        return nextTurn.UpdatedATBState;
    }

    private void ApplyTurn(GameSession gameSession, List<GameEntity> atb, CommandType commandType)
    {
        gameSession.RoundState.ATB = atb;
        gameSession.RoundState.LastCommand = commandType;
    }

    private void ApplyAttack(GameObject attacker, Unit defender)
    {
        var attackerCount = 1;
        if (attacker is Unit unitAttacker)
        {
            attackerCount = unitAttacker.Count;
        }

        var damage = _damageCalculator.CalculateDamage(new DamageContext(
            attackerAttack: attacker.Attack,
            attackerMinDamage: attacker.MinDamage,
            attackerMaxDamage: attacker.MaxDamage,
            attackerCount: attackerCount,
            defenderDefense: defender.Defence,
            damageModifier: 1)); // TODO: в будущем добавить класс для вычисления модификатора дамага по абилкам 

        var damageResult = _damageCalculator.ComputeCasualties(new DefenderTakeDamageContext(
            defenderCurrentHealth: defender.CurrentHealth,
            defenderFullHealth: defender.FullHealth,
            defenderCount: defender.Count,
            attackerDamage: damage));

        defender.Count = damageResult.SurvivingUnits;
        defender.CurrentHealth = damageResult.RemainingHealth;
    }

    private void RemoveDeathUnits(GameSession gameSession)
    {
        var deathUnitIds = gameSession.RoundState.Units.Where(unit => unit.Count <= 0).Select(unit => unit.Id);
        gameSession.RoundState.Units.RemoveAll(unit => deathUnitIds.Contains(unit.Id));
        gameSession.RoundState.ATB.RemoveAll(item => deathUnitIds.Contains(item.GameEntityId));
        foreach (var player in gameSession.Players)
        {
            player.Units.RemoveAll(unit => deathUnitIds.Contains(unit.Id));
        }
    }
}
