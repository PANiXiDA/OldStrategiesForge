using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tools.AWS3.Configuration;
using Tools.AWS3.Interfaces;

namespace Tools.AWS3.Extensions;

public static class AWSExtensions
{
    public static IServiceCollection UseAWS3(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAWSService<IAmazonS3>();
        var awsConf = configuration.GetAWSOptions("AWS");
        awsConf.Credentials = new BasicAWSCredentials(configuration.GetSection("AWS:Credentials:AccessKey").Get<string>()
            , configuration.GetSection("AWS:Credentials:SecretKey").Get<string>());
        serviceCollection.AddDefaultAWSOptions(awsConf);
        serviceCollection.Configure<AmazonS3BucketOptions>(configuration.GetSection(nameof(AmazonS3BucketOptions)));
        serviceCollection.AddScoped<IS3Client, BaseS3Client>();

        return serviceCollection;
    }
}