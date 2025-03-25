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

        public List<UnitPosition> SetStartingPosition(IEnumerable<UnitInitiative> unitInitiatives)
        {
            var startingPositions = new List<UnitPosition>();

            foreach (var unit in unitInitiatives)
            {
                startingPositions.Add(new UnitPosition
                {
                    UnitId = unit.UnitId,
                    Position = _random.NextDouble() * 15
                });
            }

            return startingPositions;
        }

        public List<UnitPosition> ShiftATBPosition(ATBPositionShiftContext request)
        {
            var unitState = request.CurrentATBState.First(state => state.UnitId ==
                request.UnitId);
            unitState.Position += unitState.Position * request.ShiftFactor;

            return request.CurrentATBState;
        }

        public ATBNextTurnResult GetNextTurn(ATBCalculationContext context)
        {
            var finishingTimes = CalculateFinishingTimes(context.UnitInitiatives, context.CurrentATBState);
            var nextUnit = ApplyTurnUpdate(
                context.UnitInitiatives,
                context.CurrentATBState,
                finishingTimes);

            return new ATBNextTurnResult
            {
                NextUnitId = nextUnit.UnitId,
                UpdatedATBState = context.CurrentATBState
            };
        }

        public List<Guid> PredictNextTurns(ATBCalculationContext context, int countTurns = 100)
        {
            List<UnitPosition> simulatedATBState = context.CurrentATBState
                .Select(state => new UnitPosition
                {
                    UnitId = state.UnitId,
                    Position =
                    state.Position
                })
                .ToList();

            List<Guid> turnOrder = new List<Guid>();

            for (int i = 0; i < countTurns; i++)
            {
                var finishingTimes = CalculateFinishingTimes(context.UnitInitiatives, simulatedATBState);
                var nextUnit = ApplyTurnUpdate(
                    context.UnitInitiatives,
                    simulatedATBState,
                    finishingTimes);
                turnOrder.Add(nextUnit.UnitId);
            }

            return turnOrder;
        }

        private List<UnitFinishingTime> CalculateFinishingTimes(
            List<UnitInitiative> unitInitiatives,
            List<UnitPosition> currentStates)
        {
            var finishingTimes = new List<UnitFinishingTime>();

            foreach (var unit in unitInitiatives)
            {
                var state = currentStates.First(currentState => currentState.UnitId == unit.UnitId);
                double time = unit.Initiative > 0 
                    ? (100 - state.Position) / unit.Initiative
                    : double.PositiveInfinity;

                finishingTimes.Add(new UnitFinishingTime
                {
                    UnitId = unit.UnitId,
                    FinishingTime = time
                });
            }

            return finishingTimes;
        }

        private UnitFinishingTime ApplyTurnUpdate(
            List<UnitInitiative> unitInitiatives,
            List<UnitPosition> currentStates, 
            List<UnitFinishingTime> finishingTimes)
        {
            var nextUnit = finishingTimes.OrderBy(finishingTime =>
                finishingTime.FinishingTime).First();
            double minTime = nextUnit.FinishingTime;

            foreach (var unit in unitInitiatives)
            {
                var state = currentStates.First(currentState => currentState.UnitId == unit.UnitId);
                state.Position += unit.Initiative * minTime;
            }

            var nextUnitState = currentStates.First(state => state.UnitId == nextUnit.UnitId);
            nextUnitState.Position -= 100;

            return nextUnit;
        }
    }
}
