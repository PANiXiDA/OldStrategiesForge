using System;

namespace GameEngine.Domains
{
    public class Hero
    {
        public Guid Id { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public double Initiative { get; set; }

        public Hero(
            Guid id,
            int attack,
            int defence,
            int minDamage,
            int maxDamage,
            double initiative)
        {
            Id = id;
            Attack = attack;
            Defence = defence;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Initiative = initiative;
        }
    }
}
