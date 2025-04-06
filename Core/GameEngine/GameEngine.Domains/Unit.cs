using GameEngine.Domains.Core;
using System;
using System.Collections.Generic;

namespace GameEngine.Domains
{
    public class Unit : GameObject
    {
        public int FullHealth { get; set; }
        public int CurrentHealth { get; set; }
        public double CurrentInitiative { get; set; }
        public int Speed { get; set; }
        public int? Range { get; set; }
        public int? Arrows { get; set; }
        public int Count { get; set; }

        public List<Ability> Abilities { get; set; }
        public List<Effect> Effects { get; set; }

        public Unit(
            Guid id,
            int attack,
            int defence,
            int fullHealth,
            int minDamage,
            int maxDamage,
            double initiative,
            int speed,
            int? range,
            int? arrows,
            int morale,
            int luck,
            int count,
            List<Ability> abilities,
            List<Effect> effects) : base(id, attack, defence, minDamage, maxDamage, initiative, morale, luck)
        {
            CurrentInitiative = initiative;
            FullHealth = fullHealth;
            CurrentHealth = fullHealth;
            Speed = speed;
            Range = range;
            Arrows = arrows;
            Count = count;
            Abilities = abilities;
            Effects = effects;
        }
    }
}
