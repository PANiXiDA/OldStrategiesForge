using GameEngine.DTO.ATBCalculator;
using System.Collections.Generic;
using System;
using System.Linq;
using GameEngine.Interfaces;

namespace GameEngine.Implementations
{
    public class ATBCalculator : IATBCalculator
    {
        private readonly Random _random;

        public ATBCalculator()
        {
            _random = new Random();
        }

        public List<GameEntity> SetStartingPosition(IEnumerable<GameEntityInitiative> gameEntityInitiatives)
        {
            var startingPositions = new List<GameEntity>();

            foreach (var unit in gameEntityInitiatives)
            {
                startingPositions.Add(new GameEntity
                {
                    GameEntityId = unit.GameEntityId,
                    Position = _random.NextDouble() * 15
                });
            }

            return startingPositions;
        }

        public List<GameEntity> ShiftATBPosition(ATBPositionShiftContext request)
        {
            var unitState = request.CurrentATBState.First(state => state.GameEntityId ==
                request.UnitId);
            unitState.Position += unitState.Position * request.ShiftFactor;

            return request.CurrentATBState;
        }

        public ATBNextTurnResult GetNextTurn(ATBCalculationContext context)
        {
            var finishingTimes = CalculateFinishingTimes(context.GameEntitiesInitiatives, context.CurrentATBState);
            var nextUnit = ApplyTurnUpdate(
                context.GameEntitiesInitiatives,
                context.CurrentATBState,
                finishingTimes);

            return new ATBNextTurnResult
            {
                NextGameObjectId = nextUnit.UnitId,
                UpdatedATBState = context.CurrentATBState
            };
        }

        public List<Guid> PredictNextTurns(ATBCalculationContext context, int countTurns = 100)
        {
            List<GameEntity> simulatedATBState = context.CurrentATBState
                .Select(state => new GameEntity
                {
                    GameEntityId = state.GameEntityId,
                    Position =
                    state.Position
                })
                .ToList();

            List<Guid> turnOrder = new List<Guid>();

            for (int i = 0; i < countTurns; i++)
            {
                var finishingTimes = CalculateFinishingTimes(context.GameEntitiesInitiatives, simulatedATBState);
                var nextUnit = ApplyTurnUpdate(
                    context.GameEntitiesInitiatives,
                    simulatedATBState,
                    finishingTimes);
                turnOrder.Add(nextUnit.UnitId);
            }

            return turnOrder;
        }

        private List<UnitFinishingTime> CalculateFinishingTimes(
            List<GameEntityInitiative> gameEntityInitiatives,
            List<GameEntity> currentStates)
        {
            var finishingTimes = new List<UnitFinishingTime>();

            foreach (var unit in gameEntityInitiatives)
            {
                var state = currentStates.First(currentState => currentState.GameEntityId == unit.GameEntityId);
                double time = unit.Initiative > 0 
                    ? (100 - state.Position) / unit.Initiative
                    : double.PositiveInfinity;

                finishingTimes.Add(new UnitFinishingTime
                {
                    UnitId = unit.GameEntityId,
                    FinishingTime = time
                });
            }

            return finishingTimes;
        }

        private UnitFinishingTime ApplyTurnUpdate(
            List<GameEntityInitiative> gameEntityInitiatives,
            List<GameEntity> currentStates, 
            List<UnitFinishingTime> finishingTimes)
        {
            var nextUnit = finishingTimes.OrderBy(finishingTime =>
                finishingTime.FinishingTime).First();
            double minTime = nextUnit.FinishingTime;

            foreach (var unit in gameEntityInitiatives)
            {
                var state = currentStates.First(currentState => currentState.GameEntityId == unit.GameEntityId);
                state.Position += unit.Initiative * minTime;
            }

            var nextUnitState = currentStates.First(state => state.GameEntityId == nextUnit.UnitId);
            nextUnitState.Position -= 100;

            return nextUnit;
        }
    }
}
