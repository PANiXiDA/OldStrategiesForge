using System.Collections.Generic;
using System;

namespace GameEngine.DTO.ATBCalculator
{
    public class ATBNextTurnResult
    {
        public Guid NextUnitId { get; set; }
        public List<UnitPosition> UpdatedATBState { get; set; }
    }
}
