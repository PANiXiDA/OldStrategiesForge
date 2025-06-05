using System;
using System.Threading;
using System.Threading.Tasks;
using GamesService.Services;
using GamesService.DAL.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using ImageService.S3Images.Gen;
using Matchmaking.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Profile.Players.Gen;
using Xunit;

public class MatchmakingServiceTests : IDisposable
{
    private readonly Server _server;
    private readonly GrpcChannel _channel;
    private readonly GameMatchmaking.GameMatchmakingClient _client;

    public MatchmakingServiceTests()
    {
        var logger = NullLogger<MatchmakingServiceImpl>.Instance;
        var playersClient = new Mock<ProfilePlayers.ProfilePlayersClient>();
        playersClient.Setup(p => p.GetAsync(It.IsAny<GetPlayerRequest>(), null, null, It.IsAny<CancellationToken>()))
            .Returns(new AsyncUnaryCall<GetPlayerResponse>(Task.FromResult(new GetPlayerResponse
            {
                Id = 1,
                Nickname = "Test",
                Avatar = new Avatar { S3Path = "a" },
                Frame = new Frame { S3Path = "f" },
                Mmr = 1500,
                Rank = 1,
                Level = 1
            }), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { }));

        var s3Client = new Mock<S3Images.S3ImagesClient>();
        s3Client.Setup(s => s.GetPresignedUrlAsync(It.IsAny<GetPresignedUrlRequest>(), null, null, It.IsAny<CancellationToken>()))
            .Returns(new AsyncUnaryCall<GetPresignedUrlResponse>(Task.FromResult(new GetPresignedUrlResponse { FileUrls = { "url" } }), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { }));

        var scopeFactory = new Mock<IServiceScopeFactory>();
        var scope = new Mock<IServiceScope>();
        var provider = new ServiceCollection().AddSingleton(new Mock<IGamesDAL>().Object).AddSingleton(new Mock<ISessionsDAL>().Object).BuildServiceProvider();
        scope.SetupGet(s => s.ServiceProvider).Returns(provider);
        scopeFactory.Setup(f => f.CreateScope()).Returns(scope.Object);

        var service = new MatchmakingServiceImpl(logger, playersClient.Object, s3Client.Object, scopeFactory.Object);
        _server = new Server
        {
            Services = { GameMatchmaking.BindService(service) },
            Ports = { new ServerPort("localhost", 50055, ServerCredentials.Insecure) }
        };
        _server.Start();

        _channel = GrpcChannel.ForAddress("http://localhost:50055");
        _client = new GameMatchmaking.GameMatchmakingClient(_channel);
    }

    [Fact]
    public async Task Matchmaking_Works()
    {
        using var cts = new CancellationTokenSource(100);
        var call = _client.Matchmaking(new MatchmakingRequest { PlayerId = 1 }, cancellationToken: cts.Token);
        await Assert.ThrowsAnyAsync<Exception>(async () => await call.ResponseStream.MoveNext(cts.Token));
    }

    public void Dispose()
    {
        _server.ShutdownAsync().Wait();
        _channel.Dispose();
    }
}
