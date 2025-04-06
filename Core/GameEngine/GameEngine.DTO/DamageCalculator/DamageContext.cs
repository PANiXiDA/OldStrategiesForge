namespace GameEngine.DTO.DamageCalculator
{
    public class DamageContext
    {
        public int AttackerAttack { get; set; }
        public int AttackerMinDamage { get; set; }
        public int AttackerMaxDamage { get; set; }
        public int AttackerCount { get; set; }
        public int DefenderDefense { get; set; }
        public int DamageModifier { get; set; }

        public DamageContext(
            int attackerAttack,
            int attackerMinDamage,
            int attackerMaxDamage,
            int attackerCount,
            int defenderDefense,
            int damageModifier)
        {
            AttackerAttack = attackerAttack;
            AttackerMinDamage = attackerMinDamage;
            AttackerMaxDamage = attackerMaxDamage;
            AttackerCount = attackerCount;
            DefenderDefense = defenderDefense;
            DamageModifier = damageModifier;
        }
    }
}
