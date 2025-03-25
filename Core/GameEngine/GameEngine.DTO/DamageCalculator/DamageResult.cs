namespace GameEngine.DTO.DamageCalculator
{
    public class DamageResult
    {
        public int DeadUnits { get; set; }
        public int SurvivingUnits { get; set; }
        public int RemainingHealth { get; set; }

        public DamageResult(
            int deadUnits,
            int survivingUnits,
            int remainingHealth)
        {
            DeadUnits = deadUnits;
            SurvivingUnits = survivingUnits;
            RemainingHealth = remainingHealth;
        }
    }
}
