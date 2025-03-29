# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/GamePlayService/GamePlayService.csproj", "Services/GamePlayService/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Core/Contracts/Contracts.csproj", "Core/Contracts/"]
COPY ["Core/GameEngine/GameEngine.Domains/GameEngine.Domains.csproj", "Core/GameEngine/GameEngine.Domains/"]
COPY ["Core/GameEngine/GameEngine.DTO/GameEngine.DTO.csproj", "Core/GameEngine/GameEngine.DTO/"]
COPY ["Core/GameEngine/GameEngine.Interfaces/GameEngine.Interfaces.csproj", "Core/GameEngine/GameEngine.Interfaces/"]
COPY ["Core/GameEngine/GameEngineDotnetDI/GameEngineDotnetDI.csproj", "Core/GameEngine/GameEngineDotnetDI/"]
COPY ["Core/GameEngine/GameEngine.Implementations/GameEngine.Implementations.csproj", "Core/GameEngine/GameEngine.Implementations/"]
COPY ["Tools/Tools.csproj", "Tools/"]
RUN dotnet restore "./Services/GamePlayService/GamePlayService.csproj"
COPY . .
WORKDIR "/src/Services/GamePlayService"
RUN dotnet build "./GamePlayService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GamePlayService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GamePlayService.dll"]