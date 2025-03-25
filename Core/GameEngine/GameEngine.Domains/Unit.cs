using System;

namespace GameEngine.Domains
{
    public class Unit
    {
        public Guid Id { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public double Initiative { get; set; }
        public int Morale { get; set; }
        public int Luck { get; set; }
        public double BaseInitiative { get; set; }
        public double CurrentInitiative { get; set; }

        public Unit(
            Guid id,
            int attack,
            int defence,
            int minDamage,
            int maxDamage,
            double initiative,
            int morale,
            int luck,
            double baseInitiative,
            double currentInitiative)
        {
            Id = id;
            Attack = attack;
            Defence = defence;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Initiative = initiative;
            Morale = morale;
            Luck = luck;
            BaseInitiative = baseInitiative;
            CurrentInitiative = currentInitiative;
        }
    }
}
