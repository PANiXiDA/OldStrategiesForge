using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatsService.Services;
using ChatsService.DAL.Interfaces;
using Global.Chat.Gen;
using Grpc.Core;
using Grpc.Net.Client;
using ImageService.S3Images.Gen;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Profile.Players.Gen;
using Xunit;

public class ChatServiceTests : IDisposable
{
    private readonly Server _server;
    private readonly GrpcChannel _channel;
    private readonly GlobalChat.GlobalChatClient _client;

    public ChatServiceTests()
    {
        var logger = NullLogger<GlobalChatServiceImpl>.Instance;
        var playersClient = new Mock<ProfilePlayers.ProfilePlayersClient>();
        playersClient.Setup(p => p.GetAsync(It.IsAny<GetPlayerRequest>(), null, null, It.IsAny<CancellationToken>()))
            .Returns(new AsyncUnaryCall<GetPlayerResponse>(Task.FromResult(new GetPlayerResponse
            {
                Id = 1,
                Nickname = "Test",
                Avatar = new Avatar { S3Path = "a" },
                Frame = new Frame { S3Path = "f" }
            }), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { }));

        var s3Client = new Mock<S3Images.S3ImagesClient>();
        s3Client.Setup(s => s.GetPresignedUrlAsync(It.IsAny<GetPresignedUrlRequest>(), null, null, It.IsAny<CancellationToken>()))
            .Returns(new AsyncUnaryCall<GetPresignedUrlResponse>(Task.FromResult(new GetPresignedUrlResponse { FileUrls = { "url" } }), Task.FromResult(new Metadata()), () => Status.DefaultSuccess, () => new Metadata(), () => { }));

        var chatsDal = new Mock<IChatsDAL>();
        chatsDal.Setup(c => c.GetAsync(It.IsAny<Common.SearchParams.ChatsService.ChatsSearchParams>(), It.IsAny<Common.ConvertParams.ChatsService.ChatsConvertParams>()))
            .ReturnsAsync(new Common.SearchParams.Core.SearchResult<ChatsService.Dto.ChatDto> { Objects = new List<ChatsService.Dto.ChatDto> { new ChatsService.Dto.ChatDto(Common.Enums.ChatType.Global, "Global") } });
        var messagesDal = new Mock<IMessagesDAL>();
        messagesDal.Setup(m => m.GetAsync(It.IsAny<Common.SearchParams.ChatsService.MessagesSearchParams>(), It.IsAny<Common.ConvertParams.ChatsService.MessagesConvertParams>()))
            .ReturnsAsync(new Common.SearchParams.Core.SearchResult<ChatsService.Dto.MessageDto>());
        messagesDal.Setup(m => m.AddOrUpdateAsync(It.IsAny<ChatsService.Dto.MessageDto>())).ReturnsAsync(Guid.NewGuid());

        var service = new GlobalChatServiceImpl(logger, playersClient.Object, s3Client.Object, chatsDal.Object, messagesDal.Object);
        _server = new Server
        {
            Services = { GlobalChat.BindService(service) },
            Ports = { new ServerPort("localhost", 50052, ServerCredentials.Insecure) }
        };
        _server.Start();

        _channel = GrpcChannel.ForAddress("http://localhost:50052");
        _client = new GlobalChat.GlobalChatClient(_channel);
    }

    [Fact]
    public async Task ChatStream_Works()
    {
        using var cts = new CancellationTokenSource(100);
        var call = _client.ChatStream(cancellationToken: cts.Token);
        await Assert.ThrowsAnyAsync<Exception>(async () => await call.ResponseStream.MoveNext(cts.Token));
    }

    [Fact]
    public async Task GetHistory_Works()
    {
        var response = await _client.GetHistoryAsync(new Google.Protobuf.WellKnownTypes.Empty());
        Assert.NotNull(response);
    }

    public void Dispose()
    {
        _server.ShutdownAsync().Wait();
        _channel.Dispose();
    }
}
