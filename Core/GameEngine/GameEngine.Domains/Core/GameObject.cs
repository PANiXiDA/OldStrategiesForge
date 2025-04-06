using System;

namespace GameEngine.Domains.Core
{
    public class GameObject
    {
        public Guid Id { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public double Initiative { get; set; }
        public int Morale { get; set; }
        public int Luck { get; set; }

        public GameObject(
            Guid id,
            int attack,
            int defence,
            int minDamage,
            int maxDamage,
            double initiative,
            int morale,
            int luck)
        {
            Id = id;
            Attack = attack;
            Defence = defence;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Initiative = initiative;
            Morale = morale;
            Luck = luck;
        }
    }
}
