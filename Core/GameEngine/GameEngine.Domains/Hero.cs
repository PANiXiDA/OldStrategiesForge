using GameEngine.Domains.Core;
using System;

namespace GameEngine.Domains
{
    public class Hero : GameObject
    {
        public Hero(
            Guid id,
            int attack,
            int defence,
            int minDamage,
            int maxDamage,
            double initiative,
            int morale,
            int luck) : base(id, attack, defence, minDamage, maxDamage, initiative, morale, luck)
        {
        }
    }
}
