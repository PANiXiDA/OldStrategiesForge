using System;
using System.Collections.Generic;

namespace GameEngine.Domains
{
    public class Unit
    {
        public Guid Id { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public double BaseInitiative { get; set; }
        public double CurrentInitiative { get; set; }
        public int Morale { get; set; }
        public int Luck { get; set; }
        public int Count { get; set; }

        public List<Ability> Abilities { get; set; }
        public List<Effect> Effects { get; set; }

        public Unit(
            Guid id,
            int attack,
            int defence,
            int minDamage,
            int maxDamage,
            double baseInitiative,
            int morale,
            int luck,
            int count,
            List<Ability> abilities,
            List<Effect> effects)
        {
            Id = id;
            Attack = attack;
            Defence = defence;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            BaseInitiative = baseInitiative;
            Morale = morale;
            Luck = luck;
            CurrentInitiative = baseInitiative;
            Count = count;
            Abilities = abilities;
            Effects = effects;
        }
    }
}
