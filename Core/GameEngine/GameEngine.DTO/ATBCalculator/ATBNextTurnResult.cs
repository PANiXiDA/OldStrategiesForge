using System.Collections.Generic;
using System;

namespace GameEngine.DTO.ATBCalculator
{
    public class ATBNextTurnResult
    {
        public Guid NextUnitId { get; set; }
        public List<GameEntity> UpdatedATBState { get; set; }
    }
}
