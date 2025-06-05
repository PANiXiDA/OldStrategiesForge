using System;
using System.Threading.Tasks;
using GameData.Competencies.Gen;
using GameData.Entities.Gen;
using GameDataService.Services;
using GameDataService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

public class CompetenciesServiceTests : IDisposable
{
    private readonly Server _server;
    private readonly GrpcChannel _channel;
    private readonly CompetenciesService.CompetenciesServiceClient _client;

    public CompetenciesServiceTests()
    {
        var logger = NullLogger<CompetenciesServiceImpl>.Instance;
        var dal = new Mock<ICompetenciesDAL>();
        dal.Setup(d => d.GetAsync(It.IsAny<int>(), It.IsAny<GameData.ConvertParams.Gen.CompetenciesConvertParams>()))
            .ReturnsAsync(new Competence());
        dal.Setup(d => d.GetAsync(It.IsAny<GameData.SearchParams.Gen.CompetenciesSearchParams>(), It.IsAny<GameData.ConvertParams.Gen.CompetenciesConvertParams>()))
            .ReturnsAsync(new Common.SearchParams.Core.SearchResult<Competence>());
        dal.Setup(d => d.AddOrUpdateAsync(It.IsAny<Competence>())).ReturnsAsync(1);
        dal.Setup(d => d.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

        var service = new CompetenciesServiceImpl(logger, dal.Object);
        _server = new Server
        {
            Services = { CompetenciesService.BindService(service) },
            Ports = { new ServerPort("localhost", 50054, ServerCredentials.Insecure) }
        };
        _server.Start();

        _channel = GrpcChannel.ForAddress("http://localhost:50054");
        _client = new CompetenciesService.CompetenciesServiceClient(_channel);
    }

    [Fact]
    public async Task Get_Works()
    {
        var response = await _client.GetAsync(new GetCompetenceRequest { Id = 1 });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task GetByFilter_Works()
    {
        var response = await _client.GetByFilterAsync(new GetCompetenciesRequest());
        Assert.NotNull(response);
    }

    [Fact]
    public async Task CreateOrUpdate_Works()
    {
        var response = await _client.CreateOrUpdateAsync(new Competence());
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Delete_Works()
    {
        var response = await _client.DeleteAsync(new DeleteCompetenceRequest { Id = 1 });
        Assert.NotNull(response);
    }

    public void Dispose()
    {
        _server.ShutdownAsync().Wait();
        _channel.Dispose();
    }
}
