namespace Common.Constants;

public static class ServicePorts
{
    private static readonly Dictionary<string, (int HttpPort, int GrpcPort)> PortsMap = new()
    {
        { ServiceNames.APIGateway, (PortsConstants.APIGatewayHttpPort, PortsConstants.APIGatewayGrpcPort) },
        { ServiceNames.ProfileService, (PortsConstants.ProfileServiceHttpPort, PortsConstants.ProfileServiceGrpcPort) },
        { ServiceNames.ImagesService, (PortsConstants.ImagesServiceHttpPort, PortsConstants.ImageServiceGrpcPort) },
        { ServiceNames.ChatsService, (PortsConstants.ChatsServiceHttpPort, PortsConstants.ChatsServiceGrpcPort) },
        { ServiceNames.GamesService, (PortsConstants.GamesServiceHttpPort, PortsConstants.GamesServiceGrpcPort) },
        { ServiceNames.GameDataService, (PortsConstants.GameDataServiceHttpPort, PortsConstants.GameDataServiceGrpcPort) },
        { ServiceNames.PlayerBuildsService, (PortsConstants.PlayerBuildsServiceHttpPort, PortsConstants.PlayerBuildsServiceGrpcPort) },
        { ServiceNames.FrontendService, (PortsConstants.FrontendServiceHttpPort, 0) }
    };

    public static (int HttpPort, int GrpcPort) GetPorts(string serviceName)
    {
        if (PortsMap.TryGetValue(serviceName, out var ports))
        {
            return ports;
        }

        throw new ArgumentException($"Service {serviceName} is not configured.", nameof(serviceName));
    }
}
