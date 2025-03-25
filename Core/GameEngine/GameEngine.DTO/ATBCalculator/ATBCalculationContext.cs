using System.Collections.Generic;

namespace GameEngine.DTO.ATBCalculator
{
    public class ATBCalculationContext
    {
        public List<UnitInitiative> UnitInitiatives { get; set; }
        public List<UnitPosition> CurrentATBState { get; set; }
    }
}
