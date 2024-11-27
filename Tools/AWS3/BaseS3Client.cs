using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tools.AWS3.Configuration;
using Tools.AWS3.Interfaces;

namespace Tools.AWS3;

public class BaseS3Client : IS3Client
{
    private readonly IAmazonS3 _amazonS3;
    private readonly ILogger<BaseS3Client> _logger;
    private readonly string _bucketName;
    private const string basePrefix = "db202587-tactical-heroes";

    public BaseS3Client(IAmazonS3 amazonS3
        , IOptions<AmazonS3BucketOptions> bucketConfiguration, ILogger<BaseS3Client> logger)
    {
        _amazonS3 = amazonS3;
        _logger = logger;
        _bucketName = bucketConfiguration.Value.BucketName;
    }

    public async Task<string?> PutFile(Stream stream, string fileExtension, string fileType)
    {
        return await PutFile(stream, basePrefix, fileExtension, fileType);
    }

    public async Task<string?> PutFile(Stream stream, string keyPrefix, string fileExtension, string fileType)
    {
        var key = $"{keyPrefix.TrimEnd('/')}/{Guid.NewGuid()}.{fileExtension.TrimStart('.')}";

        var request = new PutObjectRequest()
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream
        };
        request.Metadata.Add("Content-Type", fileType);

        try
        {
            var response = await _amazonS3.PutObjectAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Неудалось отправить файл в S3. Bucket: {_bucketName}");
            return null;
        }

        return key;
    }

    public async Task<GetObjectResponse> GetFile(string key)
    {
        var s3Object = await _amazonS3.GetObjectAsync(_bucketName, key);
        return s3Object;
    }

    public async Task<bool> DeleteFile(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
        };
        try
        {
            var deleteResponse = await _amazonS3.DeleteObjectAsync(deleteRequest);
            return deleteResponse.HttpStatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Неудалось удалить файла из S3. Bucket: {_bucketName}");
            return false;
        }
    }

    public async Task<string> GeneratePresignedUrl(string key, TimeSpan expirationTime, HttpVerb verb = HttpVerb.GET)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.Add(expirationTime),
                Verb = verb
            };

            string presignedUrl = await _amazonS3.GetPreSignedURLAsync(request);
            _logger.LogInformation($"Асинхронная временная ссылка для ключа {key} сгенерирована: {presignedUrl}");
            return presignedUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при асинхронной генерации временной ссылки для ключа {key}");
            throw;
        }
    }
}