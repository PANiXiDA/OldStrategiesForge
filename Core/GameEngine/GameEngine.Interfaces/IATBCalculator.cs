using GameEngine.DTO.ATBCalculator;
using System;
using System.Collections.Generic;

namespace GameEngine.Interfaces
{
    public interface IATBCalculator
    {
        List<UnitPosition> SetStartingPosition(IEnumerable<UnitInitiative> unitInitiatives);
        List<UnitPosition> ShiftATBPosition(ATBPositionShiftContext request);
        ATBNextTurnResult GetNextTurn(ATBCalculationContext context);
        List<Guid> PredictNextTurns(ATBCalculationContext context, int countTurns = 100);
    }
}
