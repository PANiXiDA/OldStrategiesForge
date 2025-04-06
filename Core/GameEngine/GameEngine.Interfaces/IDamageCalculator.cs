using GameEngine.DTO.DamageCalculator;

namespace GameEngine.Interfaces
{
    public interface IDamageCalculator
    {
        int CalculateDamage(DamageContext context);
        DamageResult ComputeCasualties(DefenderTakeDamageContext context);
    }
}
