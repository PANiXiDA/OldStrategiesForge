using GameEngine.DTO.DamageCalculator;
using GameEngine.Interfaces;
using System;

namespace GameEngine.Implementations
{
    public class DamageCalculator : IDamageCalculator
    {
        private const int MinimalStatsModifier = 1;
        private const double StatsModifierCoefficient = 0.05;
        private const double Epsilon = 1e-9;

        private static readonly Random _random = new Random();

        public int CalculateDamage(DamageContext context)
        {
            double baseRandomDamage = context.AttackerMinDamage
                + _random.NextDouble() * (context.AttackerMaxDamage - context.AttackerMinDamage) 
                + Epsilon;

            int delta = context.AttackerAttack - context.DefenderDefense;
            double multiplier = Math.Pow(MinimalStatsModifier
                + StatsModifierCoefficient * Math.Abs(delta), Math.Sign(delta));

            int damageOneObject = Convert.ToInt32(baseRandomDamage * multiplier);
            int damageManyObjects = context.AttackerCount * damageOneObject;

            int finalDamage = Convert.ToInt32(damageManyObjects * context.DamageModifier);

            return finalDamage;
        }

        public DamageResult ComputeCasualties(DefenderTakeDamageContext context)
        {
            int damageLeft = context.AttackerDamage;
            int deadUnits = 0;
            int remainingHealth = 0;

            if (damageLeft >= context.DefenderCurrentHealth)
            {
                damageLeft -= context.DefenderCurrentHealth;
                deadUnits++;
            }
            else
            {
                remainingHealth = context.DefenderCurrentHealth - damageLeft;
                damageLeft = 0;
            }

            if (damageLeft > 0)
            {
                int fullUnitDeaths = damageLeft / context.DefenderFullHealth;
                deadUnits += fullUnitDeaths;
                damageLeft %= context.DefenderFullHealth;
            }

            int survivingUnits = Math.Max(context.DefenderCount - deadUnits, 0);

            if (survivingUnits > 0)
            {
                remainingHealth = context.DefenderFullHealth - damageLeft;
            }
            else
            {
                remainingHealth = 0;
            }

            return new DamageResult(
                deadUnits: deadUnits,
                survivingUnits: survivingUnits,
                remainingHealth: remainingHealth
            );
        }
    }
}
