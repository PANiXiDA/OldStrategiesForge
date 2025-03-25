using GameData.Entities.Gen;
using GameData.Enums.Gen;
using Grpc.Core;
using PlayerBuilds.Gen;

namespace PlayerBuildsService.Services;

public class PlayerBuildsServiceImpl : PlayerBuilds.Gen.PlayerBuildsService.PlayerBuildsServiceBase
{
    private readonly ILogger<PlayerBuildsServiceImpl> _logger;

    public PlayerBuildsServiceImpl(ILogger<PlayerBuildsServiceImpl> logger)
    {
        _logger = logger;
    }

    public override async Task<GetPlayerBuildResponse> Get(GetPlayerBuildRequest request, ServerCallContext context)
    {

    }

    private GetPlayerBuildResponse MockPlayerBuild(string id)
    {
        return First();
    }

    private GetPlayerBuildResponse First()
    {
        var faction = new Faction
        {
            Id = 1,
            Name = "Test",
            Description = "Test"
        };

        var subfaction = new Subfaction
        {
            Id = 1,
            Name = "Test",
            Description = "Test",
            FactionId = faction.Id
        };

        var heroClass = new HeroClass
        {
            Id = 1,
            Name = "Орк крови",
            Description = "Get up"
        };

        var hero = new Hero
        {
            Id = 1,
            Name = "Abaddon216",
            Description = "Бог абадон видет все",
            Attack = 52,
            Defence = 37,
            MinDamage = 30,
            MaxDamage = 50,
            Initiative = 17.2,
            Morale = 1,
            Luck = 0,
            HeroClassId = heroClass.Id,
            HeroClass = heroClass
        };

        var fly = new Ability
        {
            Id = 1,
            Name = "Летающее существо",
            Description = "Способно преодолевать препятствия"
        };
        var flyEffect = new Effect
        {
            Id = 1,
            EffectType = EffectType.Fly1,
            Parameters = string.Empty
        };
        fly.Effects.Add(flyEffect);

        var doubleDamage = new Ability
        {
            Id = 2,
            Name = "Двойная атака",
            Description = "Существо наносит урон дважды"
        };
        var doubleDamageEffect = new Effect
        {
            Id = 2,
            EffectType = EffectType.DoubleDamage1,
            Parameters = string.Empty
        };
        doubleDamage.Effects.Add(doubleDamageEffect);

        var vampirism = new Ability
        {
            Id = 3,
            Name = "Вампиризм",
            Description = "Поглощает часть здоровья противника"
        };
        var vampirismEffect = new Effect
        {
            Id = 3,
            EffectType = EffectType.Vampirisme1,
            Parameters = string.Empty
        };
        vampirism.Effects.Add(vampirismEffect);

        var archer = new Ability
        {
            Id = 4,
            Name = "Лучник",
            Description = "Увеличенная дальность стрельбы"
        };
        var archerEffect = new Effect
        {
            Id = 4,
            EffectType = EffectType.Archer,
            Parameters = string.Empty
        };
        archer.Effects.Add(archerEffect);

        var noPenaltyInMelee = new Ability
        {
            Id = 5,
            Name = "Мастер ближнего боя",
            Description = "Не получает штраф в ближнем бою"
        };
        var noPenaltyInMeleeEffect = new Effect
        {
            Id = 5,
            EffectType = EffectType.NoPenaltyInMelee,
            Parameters = string.Empty
        };
        noPenaltyInMelee.Effects.Add(noPenaltyInMeleeEffect);

        var discardingBlow = new Ability
        {
            Id = 6,
            Name = "Отбрасывающий удар",
            Description = "Отбрасывает противника"
        };
        var discardingBlowEffect = new Effect
        {
            Id = 6,
            EffectType = EffectType.DiscardingBlow,
            Parameters = string.Empty
        };
        discardingBlow.Effects.Add(discardingBlowEffect);

        var ignoringDefence = new Ability
        {
            Id = 7,
            Name = "Игнорирование защиты",
            Description = "Игнорирует защиту противника"
        };
        var ignoringDefenceEffect = new Effect
        {
            Id = 7,
            EffectType = EffectType.IgnoringDefence,
            Value = 30,
            Parameters = string.Empty
        };
        ignoringDefence.Effects.Add(ignoringDefenceEffect);

        var attackFirst = new Ability
        {
            Id = 8,
            Name = "Первый удар",
            Description = "Наносит удар первым"
        };
        var attackFirstEffect = new Effect
        {
            Id = 8,
            EffectType = EffectType.AttackFirst,
            Parameters = string.Empty
        };
        attackFirst.Effects.Add(attackFirstEffect);

        var sniper = new Ability
        {
            Id = 9,
            Name = "Снайпер",
            Description = "Высокая точность выстрела"
        };
        var sniperEffect = new Effect
        {
            Id = 9,
            EffectType = EffectType.Sniper,
            Parameters = string.Empty
        };
        sniper.Effects.Add(sniperEffect);

        var fieryBreath = new Ability
        {
            Id = 10,
            Name = "Огненное дыхание",
            Description = "Бьет по двоим"
        };
        var fieryBreathEffect = new Effect
        {
            Id = 10,
            EffectType = EffectType.FieryBreath,
            Parameters = string.Empty
        };
        fieryBreath.Effects.Add(fieryBreathEffect);

        var undead = new Ability
        {
            Id = 11,
            Name = "Нежить",
            Description = "Обладает уникальными свойствами нежити"
        };
        var undeadEffect = new Effect
        {
            Id = 11,
            EffectType = EffectType.Undead,
            Parameters = string.Empty
        };
        undead.Effects.Add(undeadEffect);

        var areaRangeAttack = new Ability
        {
            Id = 12,
            Name = "Атака по площади",
            Description = "Атакует несколько целей одновременно"
        };
        var areaRangeAttackEffect = new Effect
        {
            Id = 12,
            EffectType = EffectType.AreaRangeAttack,
            Parameters = string.Empty
        };
        areaRangeAttack.Effects.Add(areaRangeAttackEffect);

        var bigShield = new Ability
        {
            Id = 13,
            Name = "Большой щит",
            Description = "Обеспечивает дополнительную защиту"
        };
        var bigShieldEffect = new Effect
        {
            Id = 13,
            EffectType = EffectType.BigShield,
            Value = 50,
            Parameters = string.Empty
        };
        bigShield.Effects.Add(bigShieldEffect);

        var noResponseAttack = new Ability
        {
            Id = 14,
            Name = "Неотвечаемая атака",
            Description = "Атакует, не получая ответного удара"
        };
        var noResponseAttackEffect = new Effect
        {
            Id = 14,
            EffectType = EffectType.NoResponseAttack,
            Parameters = string.Empty
        };
        noResponseAttack.Effects.Add(noResponseAttackEffect);

        var knightRunUp = new Ability
        {
            Id = 15,
            Name = "Рыцарский рывок",
            Description = "Рывок к противнику, пробивая защиту"
        };
        var knightRunUpEffect = new Effect
        {
            Id = 15,
            EffectType = EffectType.KnightRunUp,
            Value = 5,
            Parameters = string.Empty
        };
        knightRunUp.Effects.Add(knightRunUpEffect);

        var lineAttack = new Ability
        {
            Id = 16,
            Name = "Линейная атака",
            Description = "Атакует по прямой линии"
        };
        var lineAttackEffect = new Effect
        {
            Id = 16,
            EffectType = EffectType.LineAttack,
            Parameters = string.Empty
        };
        lineAttack.Effects.Add(lineAttackEffect);

        var doubleRangeAttack = new Ability
        {
            Id = 17,
            Name = "Двойная дальняя атака",
            Description = "Атакует дважды издалека"
        };
        var doubleRangeAttackEffect = new Effect
        {
            Id = 17,
            EffectType = EffectType.DoubleRangeAttack,
            Parameters = string.Empty
        };
        doubleRangeAttack.Effects.Add(doubleRangeAttackEffect);

        var godDefence = new Ability
        {
            Id = 18,
            Name = "Божественная защита",
            Description = "Обеспечивает непробиваемую защиту"
        };
        var godDefenceEffect = new Effect
        {
            Id = 18,
            EffectType = EffectType.GodDefence,
            Value = 70,
            Parameters = string.Empty
        };
        godDefence.Effects.Add(godDefenceEffect);

        var unlimitResponce = new Ability
        {
            Id = 19,
            Name = "Неограниченный ответ",
            Description = "Позволяет отвечать без ограничений"
        };
        var unlimitResponceEffect = new Effect
        {
            Id = 19,
            EffectType = EffectType.UnlimitResponce,
            Parameters = string.Empty
        };
        unlimitResponce.Effects.Add(unlimitResponceEffect);


        var goblin = new Unit
        {
            Id = 1,
            Name = "Goblin",
            Description = "Test",
            Attack = 5,
            Defence = 5,
            Health = 6,
            MinDamage = 2,
            MaxDamage = 2,
            Initiative = 11,
            Speed = 5,
            Morale = 0,
            Luck = 0
        };

        var wolfRider = new Unit
        {
            Id = 1,
            Name = "Wold Rider",
            Description = "Test",
            Attack = 10,
            Defence = 4,
            Health = 15,
            MinDamage = 3,
            MaxDamage = 5,
            Initiative = 12,
            Speed = 5,
            Morale = 0,
            Luck = 0
        };
        wolfRider.Abilities.Add(doubleDamage);

        return new GetPlayerBuildResponse();
    }
}
