namespace ProfileService.Extensions;

public static class PortsConfiguration
{
    public static int HttpPort { get; private set; }
    public static int GrpcPort { get; private set; }

    public static void ConfigurePort()
    {
        var httpPortString = Environment.GetEnvironmentVariable("HTTP_PORT");
        if (string.IsNullOrEmpty(httpPortString) || !int.TryParse(httpPortString, out var httpPort))
        {
            throw new InvalidOperationException("HTTP_PORT is not set or is not a valid integer.");
        }

        var grpcPortString = Environment.GetEnvironmentVariable("GRPC_PORT");
        if (string.IsNullOrEmpty(httpPortString) || !int.TryParse(grpcPortString, out var grpcPort))
        {
            throw new InvalidOperationException("GRPC_PORT is not set or is not a valid integer.");
        }

        HttpPort = httpPort;
        GrpcPort = grpcPort;
    }
}

