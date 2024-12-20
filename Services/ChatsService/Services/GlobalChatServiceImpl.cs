using ChatsService.DAL.Interfaces;
using Common.SearchParams.ChatsService;
using Global.Chat.Gen;
using Grpc.Core;
using Common.Enums;
using ChatsService.Dto;
using Common.Helpers;
using Profile.Players.Gen;
using ImageService.S3Images.Gen;
using Google.Protobuf.WellKnownTypes;

namespace ChatsService.Services;

public class GlobalChatServiceImpl : GlobalChat.GlobalChatBase
{
    private readonly ILogger<GlobalChatServiceImpl> _logger;
    private readonly ProfilePlayers.ProfilePlayersClient _playersClient;
    private readonly S3Images.S3ImagesClient _s3ImagesClient;

    private readonly IChatsDAL _chatsDAL;
    private readonly IMessagesDAL _messagesDAL;

    private readonly SemaphoreSlim _globalChatLock = new(1, 1);
    private readonly SemaphoreSlim _messagesQueueLock = new(1, 1);

    private Guid? _globalChatId;

    public GlobalChatServiceImpl(
        ILogger<GlobalChatServiceImpl> logger,
        ProfilePlayers.ProfilePlayersClient playersClient,
        S3Images.S3ImagesClient s3ImagesClient,
        IChatsDAL chatsDAL,
        IMessagesDAL messagesDAL)
    {
        _logger = logger;
        _playersClient = playersClient;
        _s3ImagesClient = s3ImagesClient;
        _chatsDAL = chatsDAL;
        _messagesDAL = messagesDAL;
    }

    public override async Task ChatStream(
        IAsyncStreamReader<ChatMessageRequest> requestStream,
        IServerStreamWriter<ChatMessageResponse> responseStream,
        ServerCallContext context)
    {
        if (!_globalChatId.HasValue)
        {
            await _globalChatLock.WaitAsync();
            try
            {
                var globalChat = (await _chatsDAL.GetAsync(new ChatsSearchParams
                {
                    ChatType = ChatType.Global
                })).Objects.FirstOrDefault();

                if (globalChat == null)
                {
                    _logger.LogError("Глобальный чат не был найден. Создаём новый.");
                    _globalChatId = await _chatsDAL.AddOrUpdateAsync(new ChatDto(ChatType.Global, "Global chat"));
                }
                else
                {
                    _globalChatId = globalChat.Id;
                }
            }
            finally
            {
                _globalChatLock.Release();
            }
        }

        var messagesQueue = new Queue<ChatMessageResponse>();
        var processingTasks = new List<Task>();

        var historyQueue = new Queue<ChatMessageResponse>();

        var historyMessages = (await _messagesDAL.GetAsync(new MessagesSearchParams
        {
            ChatId = _globalChatId.Value,
            GetHistoryChat = true
        })).Objects.ToList();

        var avatarPaths = historyMessages.Select(m => m.AvatarS3Path).ToList();
        var framePaths = historyMessages.Select(m => m.FrameS3Path).ToList();

        var avatarPresignedUrlsResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest
        {
            S3Paths = { avatarPaths }
        });
        var framePresignedUrlsResponse = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest
        {
            S3Paths = { framePaths }
        });

        var avatarPresignedUrls = avatarPresignedUrlsResponse.FileUrls.ToList();
        var framePresignedUrls = framePresignedUrlsResponse.FileUrls.ToList();

        for (int i = 0; i < historyMessages.Count; i++)
        {
            var message = historyMessages[i];

            historyQueue.Enqueue(new ChatMessageResponse
            {
                MessageId = message.Id.ToString(),
                SenderId = message.SenderId,
                SenderNickname = message.SenderNickname,
                Content = message.Content,
                AvatarS3Path = avatarPresignedUrls[i],
                AvatarFileName = FileNameHelper.GetFileName(message.AvatarS3Path),
                FrameS3Path = framePresignedUrls[i],
                FrameFileName = FileNameHelper.GetFileName(message.FrameS3Path),
                TimeSending = Timestamp.FromDateTime(message.CreatedAt)
            });
        }

        async Task ProcessIncomingMessages()
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                var player = await _playersClient.GetAsync(new GetPlayerRequest { Id = request.SenderId });

                var message = new MessageDto(
                    _globalChatId.Value,
                    request.Content,
                    request.SenderId,
                    player.Nickname,
                    player.Avatar.S3Path,
                    player.Frame.S3Path);

                await _messagesDAL.AddOrUpdateAsync(message);

                await _messagesQueueLock.WaitAsync();
                try
                {
                    var avatarPresignedUrl = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { message.AvatarS3Path } });
                    var framePresignedUrl = await _s3ImagesClient.GetPresignedUrlAsync(new GetPresignedUrlRequest() { S3Paths = { message.FrameS3Path } });

                    messagesQueue.Enqueue(new ChatMessageResponse
                    {
                        MessageId = message.Id.ToString(),
                        SenderId = message.SenderId,
                        SenderNickname = message.SenderNickname,
                        Content = message.Content,
                        AvatarS3Path = avatarPresignedUrl.FileUrls.First(),
                        AvatarFileName = FileNameHelper.GetFileName(message.AvatarS3Path),
                        FrameS3Path = framePresignedUrl.FileUrls.First(),
                        FrameFileName = FileNameHelper.GetFileName(message.FrameS3Path),
                        TimeSending = Timestamp.FromDateTime(message.CreatedAt)
                    });
                }
                finally
                {
                    _messagesQueueLock.Release();
                }
            }
        }

        async Task ProcessOutgoingMessages()
        {
            // Отправляем историю сообщений только текущему клиенту
            while (historyQueue.Count > 0)
            {
                var historyMessage = historyQueue.Dequeue();
                await responseStream.WriteAsync(historyMessage);
            }

            // Затем переходим к отправке новых сообщений из глобальной очереди
            while (!context.CancellationToken.IsCancellationRequested)
            {
                ChatMessageResponse? messageToSend = null;

                await _messagesQueueLock.WaitAsync();
                try
                {
                    if (messagesQueue.Count > 0)
                    {
                        messageToSend = messagesQueue.Dequeue();
                    }
                }
                finally
                {
                    _messagesQueueLock.Release();
                }

                if (messageToSend != null)
                {
                    await responseStream.WriteAsync(messageToSend);
                }
                else
                {
                    await Task.Delay(50);
                }
            }
        }

        processingTasks.Add(ProcessIncomingMessages());
        processingTasks.Add(ProcessOutgoingMessages());

        await Task.WhenAll(processingTasks);
    }

}