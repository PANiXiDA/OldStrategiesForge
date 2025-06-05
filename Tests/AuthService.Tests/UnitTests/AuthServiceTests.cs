using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Profile.Auth.Gen;
using Xunit;
using Common.Configurations;
using Common.Dto.RabbitMq;
using ProfileService.Services;
using ProfileService.DAL.Interfaces;
using Tools.RabbitMQ;
using Tools.Redis;

public class AuthServiceTests : IDisposable
{
    private readonly Server _server;
    private readonly GrpcChannel _channel;
    private readonly ProfileAuth.ProfileAuthClient _client;
    private readonly ProfileService.Dto.PlayersDto player;

    public AuthServiceTests()
    {
        var logger = NullLogger<AuthServiceImpl>.Instance;
        var jwtOptions = Options.Create(new JwtSettings
        {
            SecretKey = "test-secret-test-secret-test-secret-123456",
            Issuer = "issuer",
            Audience = "audience",
            AccessTokenExp = 1,
            RefreshTokenExp = 1
        });

        player = new ProfileService.Dto.PlayersDto { Id = 1, Email = "e", Password = Common.Helpers.Helpers.GetPasswordHash("Password1"), Confirmed = true };
        var playersDal = new Mock<IPlayersDAL>();
        playersDal.Setup(d => d.GetAsync(It.IsAny<Common.SearchParams.ProfileService.PlayersSearchParams>(), It.IsAny<Common.ConvertParams.ProfileService.PlayersConvertParams>()))
            .ReturnsAsync(new Common.SearchParams.Core.SearchResult<ProfileService.Dto.PlayersDto>());
        playersDal.Setup(d => d.AddOrUpdateAsync(It.IsAny<ProfileService.Dto.PlayersDto>()))
            .ReturnsAsync(1);
        playersDal.Setup(d => d.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(() => player);
        playersDal.Setup(d => d.GetAsync(It.IsAny<int>(), It.IsAny<Common.ConvertParams.ProfileService.PlayersConvertParams?>()))
            .ReturnsAsync(() => player);
        playersDal.Setup(d => d.UpdateLastLogin(It.IsAny<int>())).Returns(Task.CompletedTask);

        var tokensDal = new Mock<ITokensDAL>();
        tokensDal.Setup(d => d.AddOrUpdateAsync(It.IsAny<ProfileService.Dto.TokensDto>())).ReturnsAsync(1);
        tokensDal.Setup(d => d.ExistsAsync(It.IsAny<Common.SearchParams.ProfileService.TokensSearchParams>())).ReturnsAsync(false);
        tokensDal.Setup(d => d.GetAsync(It.IsAny<string>())).ReturnsAsync(new ProfileService.Dto.TokensDto { Id = 1, PlayerId = 1, RefreshToken = "r", RefreshTokenExp = DateTime.UtcNow.AddDays(1) });
        tokensDal.Setup(d => d.DeleteByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(true);
        tokensDal.Setup(d => d.DeleteByPlayerIdAsync(It.IsAny<int>())).ReturnsAsync(true);

        var rabbit = new Mock<IRabbitMQClient>();
        rabbit.Setup(r => r.CallAsync<SendEmailRequest, SendEmailResponse>(It.IsAny<SendEmailRequest>(), It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(new SendEmailResponse { Success = true });

        var redis = new Mock<IRedisCache>();
        redis.Setup(r => r.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan?>())).Returns(Task.CompletedTask);

        var service = new AuthServiceImpl(logger, jwtOptions, playersDal.Object, tokensDal.Object, rabbit.Object, redis.Object);

        _server = new Server
        {
            Services = { ProfileAuth.BindService(service) },
            Ports = { new ServerPort("localhost", 50051, ServerCredentials.Insecure) }
        };
        _server.Start();

        _channel = GrpcChannel.ForAddress("http://localhost:50051");
        _client = new ProfileAuth.ProfileAuthClient(_channel);
    }

    [Fact]
    public async Task RegistrationAsync_Works()
    {
        var response = await _client.RegistrationAsync(new RegistrationPlayerRequest
        {
            Email = "test@example.com",
            Nickname = "TestUser",
            Password = "Password1"
        });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task LoginAsync_Works()
    {
        var response = await _client.LoginAsync(new LoginPlayerRequest { Email = "e", Password = "Password1" });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ConfirmEmailAsync_Works()
    {
        player.Confirmed = false;
        var response = await _client.ConfirmEmailAsync(new ConfirmEmailRequest { Email = "e" });
        player.Confirmed = true;
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ConfirmAccountAsync_Works()
    {
        var response = await _client.ConfirmAccountAsync(new ConfirmAccountRequest { PlayerId = 1 });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task RecoveryPasswordAsync_Works()
    {
        var response = await _client.RecoveryPasswordAsync(new RecoveryPasswordRequest { Email = "e" });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ChangePasswordAsync_Works()
    {
        var response = await _client.ChangePasswordAsync(new ChangePasswordRequest { PlayerId = 1 });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task RefreshAsync_Works()
    {
        var response = await _client.RefreshAsync(new RefreshTokenRequest { RefreshToken = "r" });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task LogoutAsync_Works()
    {
        var response = await _client.LogoutAsync(new LogoutRequest { RefreshToken = "r" });
        Assert.NotNull(response);
    }

    [Fact]
    public async Task LogoutFromAllDevicesAsync_Works()
    {
        var response = await _client.LogoutFromAllDevicesAsync(new LogoutFromAllDevicesRequest { PlayerId = 1 });
        Assert.NotNull(response);
    }

    public void Dispose()
    {
        _server.ShutdownAsync().Wait();
        _channel.Dispose();
    }
}
