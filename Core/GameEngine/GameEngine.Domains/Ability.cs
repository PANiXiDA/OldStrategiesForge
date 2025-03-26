using GameEngine.Domains.Enums;
using System.Collections.Generic;

namespace GameEngine.Domains
{
    public class Ability
    {
        public AbilityType AbilityType { get; set; }
        public List<Effect> Effects { get; set; }

        public Ability(
            AbilityType abilityType,
            List<Effect> effects) 
        {
            AbilityType = abilityType;
            Effects = effects;
        }
    }
}
