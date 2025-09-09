# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Explorer.API/Explorer.API.csproj", "Explorer.API/"]
COPY ["Modules/Stakeholders/Explorer.Stakeholders.API/Explorer.Stakeholders.API.csproj", "Modules/Stakeholders/Explorer.Stakeholders.API/"]
COPY ["BuildingBlocks/Explorer.BuildingBlocks.Core/Explorer.BuildingBlocks.Core.csproj", "BuildingBlocks/Explorer.BuildingBlocks.Core/"]
COPY ["Modules/Stakeholders/Explorer.Stakeholders.Infrastructure/Explorer.Stakeholders.Infrastructure.csproj", "Modules/Stakeholders/Explorer.Stakeholders.Infrastructure/"]
COPY ["BuildingBlocks/Explorer.BuildingBlocks.Infrastructure/Explorer.BuildingBlocks.Infrastructure.csproj", "BuildingBlocks/Explorer.BuildingBlocks.Infrastructure/"]
COPY ["Modules/Stakeholders/Explorer.Stakeholders.Core/Explorer.Stakeholders.Core.csproj", "Modules/Stakeholders/Explorer.Stakeholders.Core/"]
RUN dotnet restore "./Explorer.API/Explorer.API.csproj"
COPY . .
WORKDIR "/src/Explorer.API"
RUN dotnet build "./Explorer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Explorer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Explorer.API.dll"]