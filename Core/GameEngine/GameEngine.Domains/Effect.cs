using GameEngine.Domains.Enums;

namespace GameEngine.Domains
{
    public class Effect
    {
        public EffectType EffectType { get; set; }
        public double Value { get; set; }
        public double Duration { get; set; }
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
