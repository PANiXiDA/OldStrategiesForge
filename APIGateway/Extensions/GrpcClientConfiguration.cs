using Profile.Auth.Gen;
using Profile.Players.Gen;
using Profile.Avatar.Gen;
using Profile.Frames.Gen;
using Common.Configurations;
using Common.Constants;
using ImageService.S3Images.Gen;
using Global.Chat.Gen;
using Matchmaking.Gen;
using GameData.Factions.Gen;
using GameData.Subfactions.Gen;
using GameData.Heroes.Gen;
using GameData.HeroClasses.Gen;
using GameData.Units.Gen;
using GameData.Artefacts.Gen;
using GameData.ArtefactSets.Gen;
using GameData.ArtefactSetBonuses.Gen;
using GameData.Spells.Gen;
using GameData.Competencies.Gen;
using GameData.Skills.Gen;
using GameData.Abilities.Gen;
using GameData.Effects.Gen;

namespace APIGateway.Extensions;

public static class GrpcClientConfiguration
{
    public static void ConfigureGrpcClients(this IServiceCollection services)
    {
        #region ProfileService

        GrpcConfiguration.ConfigureGrpcClient<ProfilePlayers.ProfilePlayersClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileAuth.ProfileAuthClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileAvatars.ProfileAvatarsClient>(services, ServiceNames.ProfileService);
        GrpcConfiguration.ConfigureGrpcClient<ProfileFrames.ProfileFramesClient>(services, ServiceNames.ProfileService);

        #endregion

        GrpcConfiguration.ConfigureGrpcClient<S3Images.S3ImagesClient>(services, ServiceNames.ImagesService);
        GrpcConfiguration.ConfigureGrpcClient<GlobalChat.GlobalChatClient>(services, ServiceNames.ChatsService);
        GrpcConfiguration.ConfigureGrpcClient<GameMatchmaking.GameMatchmakingClient>(services, ServiceNames.GamesService);

        #region GameDataService

        GrpcConfiguration.ConfigureGrpcClient<FactionsService.FactionsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<SubfactionsService.SubfactionsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<HeroesService.HeroesServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<HeroClassesService.HeroClassesServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<UnitsService.UnitsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<ArtefactsService.ArtefactsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<ArtefactSetsService.ArtefactSetsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<ArtefactSetBonusesService.ArtefactSetBonusesServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<CompetenciesService.CompetenciesServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<SkillsService.SkillsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<SpellsService.SpellsServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<AbilitiesService.AbilitiesServiceClient>(services, ServiceNames.GameDataService);
        GrpcConfiguration.ConfigureGrpcClient<EffectsService.EffectsServiceClient>(services, ServiceNames.GameDataService);

        #endregion
    }
}
