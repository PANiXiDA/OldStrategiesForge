using System.Collections.Generic;
using System;

namespace GameEngine.DTO.ATBCalculator
{
    public class ATBPositionShiftContext
    {
        public List<GameEntity> CurrentATBState { get; set; }
        public Guid UnitId { get; set; }
        public double ShiftFactor { get; set; } = 0.5;
    }
}
