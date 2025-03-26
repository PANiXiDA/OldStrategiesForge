using GameEngine.Domains.Enums;
using System;

namespace GameEngine.Domains
{
    public class Effect
    {
        public EffectType EffectType { get; set; }
        public Double Value { get; set; }
        public Double Duration { get; set; }
        public string Parameters { get; set; }

        public Effect(
            EffectType effectType,
            double value,
            double duration,
            string parameters)
        {
            EffectType = effectType;
            Value = value;
            Duration = duration;
            Parameters = parameters;
        }
    }
}
