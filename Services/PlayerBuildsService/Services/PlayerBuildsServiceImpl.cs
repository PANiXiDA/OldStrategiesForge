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
        return MockPlayerBuild(request.Id);
    }

    private GetPlayerBuildResponse MockPlayerBuild(string id)
    {
        if (id == "1")
        {
            return First();
        }
        else
        {
            return Second();
        }
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

        var artefactSet = new ArtefactSet
        {
            Id = 1,
            Name = "Set",
            Description = "Test"
        };

        var artefactSetBonus = new ArtefactSetBonus
        {
            Id = 1,
            ArtefactSetId = 1,
            HealthBonus = 5
        };
        artefactSet.ArtefactSetBonuses.Add(artefactSetBonus);

        var artefacts = new List<Artefact>();

        var sword = new Artefact
        {
            Id = 1,
            Name = "Sword",
            Description = "Test",
            ArtefactSlot = ArtefactSlot.Weapon,
            AttackBonus = 10
        };
        artefacts.Add(sword);

        var shield = new Artefact
        {
            Id = 2,
            Name = "Shield",
            Description = "Test",
            ArtefactSlot = ArtefactSlot.Shield,
            DefenceBonus = 7
        };
        artefacts.Add(shield);

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

        var units = new List<UnitDto>();

        var goblin = new UnitDto
        {
            Unit = new Unit
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
            },
            Count = 300
        };
        units.Add(goblin);

        var wolfRider = new UnitDto
        {
            Unit = new Unit
            {
                Id = 2,
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
            },
            Count = 120
        };
        wolfRider.Unit.Abilities.Add(doubleDamage);
        units.Add(wolfRider);

        var orc = new UnitDto
        {
            Unit = new Unit
            {
                Id = 3,
                Name = "Orc",
                Description = "Test",
                Attack = 10,
                Defence = 5,
                Health = 20,
                MinDamage = 4,
                MaxDamage = 6,
                Initiative = 12,
                Speed = 5,
                Morale = 0,
                Luck = 0
            },
            Count = 70
        };
        orc.Unit.Abilities.Add(archer);
        orc.Unit.Abilities.Add(noPenaltyInMelee);
        units.Add(orc);

        var ogr = new UnitDto
        {
            Unit = new Unit
            {
                Id = 4,
                Name = "Ogr",
                Description = "Test",
                Attack = 10,
                Defence = 5,
                Health = 50,
                MinDamage = 5,
                MaxDamage = 15,
                Initiative = 10,
                Speed = 5,
                Morale = 0,
                Luck = 0
            },
            Count = 10
        };
        ogr.Unit.Abilities.Add(discardingBlow);
        units.Add(ogr);

        var ruh = new UnitDto
        {
            Unit = new Unit
            {
                Id = 5,
                Name = "Ruh",
                Description = "Test",
                Attack = 17,
                Defence = 12,
                Health = 65,
                MinDamage = 11,
                MaxDamage = 15,
                Initiative = 17,
                Speed = 8,
                Morale = 0,
                Luck = 0
            },
            Count = 15
        };
        ruh.Unit.Abilities.Add(fly);
        units.Add(ruh);

        var cyclop = new UnitDto
        {
            Unit = new Unit
            {
                Id = 6,
                Name = "Cyclop",
                Description = "Test",
                Attack = 20,
                Defence = 10,
                Health = 100,
                MinDamage = 19,
                MaxDamage = 28,
                Initiative = 12,
                Speed = 5,
                Morale = 0,
                Luck = 0
            },
            Count = 12
        };
        cyclop.Unit.Abilities.Add(archer);
        units.Add(cyclop);

        var bear = new UnitDto
        {
            Unit = new Unit
            {
                Id = 7,
                Name = "Bear",
                Description = "Test",
                Attack = 30,
                Defence = 20,
                Health = 210,
                MinDamage = 30,
                MaxDamage = 50,
                Initiative = 10,
                Speed = 5,
                Morale = 0,
                Luck = 0
            },
            Count = 5
        };
        bear.Unit.Abilities.Add(ignoringDefence);
        units.Add(bear);

        var response = new GetPlayerBuildResponse
        {
            Faction = faction,
            Subfaction = subfaction,
            ArtefactSet = artefactSet,
            Hero = hero,
        };
        response.Artefacts.AddRange(artefacts);
        response.Units.AddRange(units);

        return response;
    }

    private GetPlayerBuildResponse Second()
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

        var artefactSet = new ArtefactSet
        {
            Id = 1,
            Name = "Set",
            Description = "Test"
        };

        var artefactSetBonus = new ArtefactSetBonus
        {
            Id = 1,
            ArtefactSetId = 1,
            HealthBonus = 5
        };
        artefactSet.ArtefactSetBonuses.Add(artefactSetBonus);

        var artefacts = new List<Artefact>();

        var sword = new Artefact
        {
            Id = 1,
            Name = "Sword",
            Description = "Test",
            ArtefactSlot = ArtefactSlot.Weapon,
            AttackBonus = 10
        };
        artefacts.Add(sword);

        var shield = new Artefact
        {
            Id = 2,
            Name = "Shield",
            Description = "Test",
            ArtefactSlot = ArtefactSlot.Shield,
            DefenceBonus = 7
        };
        artefacts.Add(shield);

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

        var units = new List<UnitDto>();

        var sprite = new UnitDto
        {
            Unit = new Unit
            {
                Id = 101,
                Name = "Sprite",
                Description = "Test",
                Attack = 3,
                Defence = 0,
                Health = 5,
                MinDamage = 2,
                MaxDamage = 3,
                Initiative = 15,
                Speed = 7,
                Morale = 0,
                Luck = 0
            },
            Count = 200
        };
        sprite.Unit.Abilities.Add(noResponseAttack);
        units.Add(sprite);

        var pikeman = new UnitDto
        {
            Unit = new Unit
            {
                Id = 102,
                Name = "Pikeman",
                Description = "Test",
                Attack = 7,
                Defence = 3,
                Health = 15,
                MinDamage = 5,
                MaxDamage = 15,
                Initiative = 10,
                Speed = 4,
                Morale = 0,
                Luck = 0
            },
            Count = 80
        };
        pikeman.Unit.Abilities.Add(attackFirst);
        units.Add(pikeman);

        var elf = new UnitDto
        {
            Unit = new Unit
            {
                Id = 103,
                Name = "Elf",
                Description = "Test",
                Attack = 5,
                Defence = 4,
                Health = 14,
                MinDamage = 5,
                MaxDamage = 8,
                Initiative = 12,
                Speed = 4,
                Morale = 0,
                Luck = 0
            },
            Count = 60
        };
        elf.Unit.Abilities.Add(archer);
        elf.Unit.Abilities.Add(sniper);
        elf.Unit.Abilities.Add(doubleRangeAttack);
        units.Add(elf);

        var griffin = new UnitDto
        {
            Unit = new Unit
            {
                Id = 104,
                Name = "Griffin",
                Description = "Test",
                Attack = 10,
                Defence = 12,
                Health = 35,
                MinDamage = 15,
                MaxDamage = 23,
                Initiative = 12,
                Speed = 6,
                Morale = 0,
                Luck = 0
            },
            Count = 40
        };
        griffin.Unit.Abilities.Add(fly);
        griffin.Unit.Abilities.Add(unlimitResponce);
        units.Add(griffin);

        var minotaurKing = new UnitDto
        {
            Unit = new Unit
            {
                Id = 105,
                Name = "MinotaurKing",
                Description = "Test",
                Attack = 30,
                Defence = 4,
                Health = 60,
                MinDamage = 22,
                MaxDamage = 44,
                Initiative = 12,
                Speed = 6,
                Morale = 0,
                Luck = 0
            },
            Count = 14
        };
        minotaurKing.Unit.Abilities.Add(lineAttack);
        units.Add(minotaurKing);

        var paladin = new UnitDto
        {
            Unit = new Unit
            {
                Id = 106,
                Name = "Paladin",
                Description = "Test",
                Attack = 35,
                Defence = 35,
                Health = 75,
                MinDamage = 25,
                MaxDamage = 30,
                Initiative = 9,
                Speed = 6,
                Morale = 0,
                Luck = 0
            },
            Count = 10
        };
        paladin.Unit.Abilities.Add(godDefence);
        units.Add(paladin);

        var titan = new UnitDto
        {
            Unit = new Unit
            {
                Id = 106,
                Name = "Titan",
                Description = "Test",
                Attack = 50,
                Defence = 30,
                Health = 300,
                MinDamage = 30,
                MaxDamage = 50,
                Initiative = 11,
                Speed = 5,
                Morale = 0,
                Luck = 0
            },
            Count = 4
        };
        titan.Unit.Abilities.Add(archer);
        titan.Unit.Abilities.Add(noPenaltyInMelee);
        units.Add(titan);

        var response = new GetPlayerBuildResponse
        {
            Faction = faction,
            Subfaction = subfaction,
            ArtefactSet = artefactSet,
            Hero = hero,
        };
        response.Artefacts.AddRange(artefacts);
        response.Units.AddRange(units);

        return response;
    }
}
