using System.Collections.Generic;

namespace GameEngine.DTO.ATBCalculator
{
    public class ATBCalculationContext
    {
        public List<GameEntityInitiative> UnitInitiatives { get; set; }
        public List<GameEntity> CurrentATBState { get; set; }
    }
}
