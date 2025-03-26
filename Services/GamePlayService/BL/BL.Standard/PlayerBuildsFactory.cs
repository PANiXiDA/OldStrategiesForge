using GamePlayService.BL.BL.Interfaces;
using PlayerBuilds.Gen;
using Hero = GameEngine.Domains.Hero;
using Unit = GameEngine.Domains.Unit;
using Ability = GameEngine.Domains.Ability;
using Effect = GameEngine.Domains.Effect;
using FactionDto = GameData.Entities.Gen.Faction;
using SubfactionDto = GameData.Entities.Gen.Subfaction;
using ArtefactSetDto = GameData.Entities.Gen.ArtefactSet;
using ArtefactDto = GameData.Entities.Gen.Artefact;
using HeroDto = GameData.Entities.Gen.Hero;
using UnitDto = PlayerBuilds.Gen.UnitDto;
using AbilityDto = GameData.Entities.Gen.Ability;
using EffectDto = GameData.Entities.Gen.Effect;
using GameEngine.Domains.Enums;

namespace GamePlayService.BL.BL.Standard;

public class PlayerBuildsFactory : IPlayerBuildsFactory
{
    private ILogger<PlayerBuildsFactory> _logger;

    private readonly PlayerBuildsService.PlayerBuildsServiceClient _playerBuildsService;

    public PlayerBuildsFactory(
        ILogger<PlayerBuildsFactory> logger,
        PlayerBuildsService.PlayerBuildsServiceClient playerBuildsService)
    {
        _logger = logger;
        _playerBuildsService = playerBuildsService;
    }

    public async Task<(Hero, List<Unit>)> GetGameEntities(string buildId)
    {
        var build = await _playerBuildsService.GetAsync(new GetPlayerBuildRequest { Id = buildId });
        var (hero, units) = BuildGameEntities(build);
        return (hero, units);
    }

    private (Hero, List<Unit>) BuildGameEntities(GetPlayerBuildResponse build)
    {
        var hero = ConvertDtoHeroToDomain(build.Hero);
        EnrichHeroByArtefacts(hero, build.Artefacts.ToList());
        EnrichHeroByArtefactSet(hero, build.ArtefactSet);

        var units = new List<Unit>();
        foreach (var unitDto in  build.Units)
        {
            var unit = ConvertDtoUnitToDomain(unitDto);
            EnrichUnitByHero(unit, hero);
            EnrichUnitByFaction(unit, build.Faction);
            EnrichUnitBySubfaction(unit, build.Subfaction);
            units.Add(unit);
        }

        return (hero, units);
    }

    private Hero ConvertDtoHeroToDomain(HeroDto hero)
    {
        return new Hero(
            id: Guid.NewGuid(),
            attack: hero.Attack,
            defence: hero.Defence,
            minDamage: hero.MinDamage,
            maxDamage: hero.MaxDamage,
            initiative: hero.Initiative,
            morale: hero.Morale,
            luck: hero.Luck);
    }

    private Unit ConvertDtoUnitToDomain(UnitDto unit)
    {
        return new Unit(
            id: Guid.NewGuid(),
            attack: unit.Unit.Attack,
            defence: unit.Unit.Defence,
            minDamage: unit.Unit.MinDamage,
            maxDamage: unit.Unit.MaxDamage,
            baseInitiative: unit.Unit.Initiative,
            morale: unit.Unit.Morale,
            luck: unit.Unit.Luck,
            count: unit.Count,
            abilities: unit.Unit.Abilities.Select(ConvertDtoAbilityToDomain).ToList(),
            effects: new List<Effect>());
    }

    private Ability ConvertDtoAbilityToDomain(AbilityDto ability)
    {
        return new Ability(
            abilityType: (AbilityType)ability.AbilityType,
            effects: ability.Effects.Select(ConvertDtoEffectToDomain).ToList());
    }

    private Effect ConvertDtoEffectToDomain(EffectDto effect)
    {
        return new Effect(
            effectType: (EffectType)effect.EffectType,
            value: effect.Value ?? 0,
            duration: effect.Duration ?? 0,
            parameters: effect.Parameters);
    }

    private void EnrichHeroByArtefacts(Hero hero, List<ArtefactDto> artefacts)
    {
        foreach (var artefact in artefacts)
        {
            hero.Attack += artefact.AttackBonus ?? 0;
            hero.Defence += artefact.DefenceBonus ?? 0;
            hero.MinDamage += artefact.MinDamageBonus ?? 0;
            hero.MaxDamage += artefact.MaxDamageBonus ?? 0;
            hero.Initiative += artefact.InitiativeBonus ?? 0;
            hero.Morale += artefact.MoraleBonus ?? 0;
            hero.Luck += artefact.LuckBonus ?? 0;
        }
    }

    private void EnrichHeroByArtefactSet(Hero hero, ArtefactSetDto artefactSet)
    {
        foreach (var bonuses in artefactSet.ArtefactSetBonuses)
        {
            hero.Attack += bonuses.AttackBonus ?? 0;
            hero.Defence += bonuses.DefenceBonus ?? 0;
            hero.MinDamage += bonuses.MinDamageBonus ?? 0;
            hero.MaxDamage += bonuses.MaxDamageBonus ?? 0;
            hero.Initiative += bonuses.InitiativeBonus ?? 0;
            hero.Morale += bonuses.MoraleBonus ?? 0;
            hero.Luck += bonuses.LuckBonus ?? 0;
        }
    }

    private void EnrichUnitByHero(Unit unit, Hero hero)
    {
        unit.Attack += hero.Attack;
        unit.Defence += hero.Defence;
        unit.MinDamage += hero.MinDamage;
        unit.MaxDamage += hero.MaxDamage;
        unit.BaseInitiative += hero.Initiative;
        unit.CurrentInitiative += hero.Initiative;
        unit.Morale += hero.Morale;
        unit.Luck += hero.Luck;
    }

    private void EnrichUnitByFaction(Unit unit, FactionDto faction)
    {
        unit.Abilities.AddRange(faction.Abilities.Select(ConvertDtoAbilityToDomain));
    }

    private void EnrichUnitBySubfaction(Unit unit, SubfactionDto subfaction)
    {
        unit.Abilities.AddRange(subfaction.Abilities.Select(ConvertDtoAbilityToDomain));
    }
}
