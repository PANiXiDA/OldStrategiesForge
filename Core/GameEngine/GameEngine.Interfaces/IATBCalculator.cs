using GameEngine.DTO.ATBCalculator;
using System;
using System.Collections.Generic;

namespace GameEngine.Interfaces
{
    public interface IATBCalculator
    {
        List<GameEntity> SetStartingPosition(IEnumerable<GameEntityInitiative> unitInitiatives);
        List<GameEntity> ShiftATBPosition(ATBPositionShiftContext request);
        ATBNextTurnResult GetNextTurn(ATBCalculationContext context);
        List<Guid> PredictNextTurns(ATBCalculationContext context, int countTurns = 100);
    }
}
