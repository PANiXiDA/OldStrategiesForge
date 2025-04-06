namespace GameEngine.DTO.DamageCalculator
{
    public class DefenderTakeDamageContext
    {
        public int DefenderCurrentHealth { get; set; }
        public int DefenderFullHealth { get; set; }
        public int DefenderCount { get; set; }
        public int AttackerDamage { get; set; }

        public DefenderTakeDamageContext(
            int defenderCurrentHealth,
            int defenderFullHealth,
            int defenderCount,
            int attackerDamage
        )
        {
            DefenderCurrentHealth = defenderCurrentHealth;
            DefenderFullHealth = defenderFullHealth;
            DefenderCount = defenderCount;
            AttackerDamage = attackerDamage;
        }
    }
}
