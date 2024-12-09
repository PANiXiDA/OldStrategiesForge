using Grpc.Core;
using ImageService.S3Images.Gen;
using Tools.AWS3.Interfaces;

namespace ImagesService.Services;
public class S3ImagesServiceImpl : S3Images.S3ImagesBase
{
    private readonly ILogger<S3ImagesServiceImpl> _logger;
    private readonly IS3Client _client;

    public S3ImagesServiceImpl(
        ILogger<S3ImagesServiceImpl> logger,
        IS3Client client)
    {
        _logger = logger;
        _client = client;
    }

    public override async Task<GetPresignedUrlResponse> GetPresignedUrl(GetPresignedUrlRequest request, ServerCallContext context)
    {
        List<string> urls = new List<string>();
        foreach (var s3Path in request.S3Paths)
        {
            var url = await _client.GeneratePresignedUrl(s3Path, TimeSpan.FromHours(1));
            urls.Add(url);
        }

        return new GetPresignedUrlResponse() { FileUrls = { urls } };
    }
}