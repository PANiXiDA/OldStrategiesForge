using System.Collections.Generic;

namespace GameEngine.DTO.ATBCalculator
{
    public class ATBCalculationContext
    {
        public List<GameEntityInitiative> GameEntitiesInitiatives { get; set; }
        public List<GameEntity> CurrentATBState { get; set; }
    }
}
