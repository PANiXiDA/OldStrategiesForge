using System;
using System.Threading.Tasks;
using GameData.Abilities.Gen;
using GameData.Entities.Gen;
using GameDataService.Services;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

public class AbilitiesServiceTests : IDisposable
{
    private readonly Server _server;
    private readonly GrpcChannel _channel;
    private readonly AbilitiesService.AbilitiesServiceClient _client;

    public AbilitiesServiceTests()
    {
        var logger = NullLogger<AbilitiesServiceImpl>.Instance;
        var dal = new Mock<IAbilitiesDAL>();
        dal.Setup(d => d.GetAsync(It.IsAny<int>(), It.IsAny<GameData.ConvertParams.Gen.AbilitiesConvertParams>()))
            .ReturnsAsync(new Ability());
        dal.Setup(d => d.GetAsync(It.IsAny<GameData.SearchParams.Gen.AbilitiesSearchParams>(), It.IsAny<GameData.ConvertParams.Gen.AbilitiesConvertParams>()))
            .ReturnsAsync(new Common.SearchParams.Core.SearchResult<Ability>());
        dal.Setup(d => d.AddOrUpdateAsync(It.IsAny<Ability>())).ReturnsAsync(1);
        dal.Setup(d => d.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

        var service = new AbilitiesServiceImpl(logger, dal.Object);
        _server = new Server
        {
            Services = { AbilitiesService.BindService(service) },
            Ports = { new ServerPort("localhost", 50053, ServerCredentials.Insecure) }
        };
        _server.Start();

        _channel = GrpcChannel.ForAddress("http://localhost:50053");
        _client = new AbilitiesService.AbilitiesServiceClient(_channel);
    }

    [Fact]
    public async Task Get_Works()
    {
        var response = await _client.GetAsync(new GetAbilityRequest { Id = 1 });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task GetByFilter_Works()
    {
        var response = await _client.GetByFilterAsync(new GetAbilitiesRequest());
        Assert.NotNull(response);
    }

    [Fact]
    public async Task CreateOrUpdate_Works()
    {
        var response = await _client.CreateOrUpdateAsync(new Ability());
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Delete_Works()
    {
        var response = await _client.DeleteAsync(new DeleteAbilityRequest { Id = 1 });
        Assert.NotNull(response);
    }

    public void Dispose()
    {
        _server.ShutdownAsync().Wait();
        _channel.Dispose();
    }
}
