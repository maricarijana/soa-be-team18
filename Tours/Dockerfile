# Base image za runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 90 

# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiranje csproj fajlova za restore
COPY ["Tours.API/Tours.API.csproj", "Tours.API/"]
COPY ["Tours.Application/Tours.Application.csproj", "Tours.Application/"]
COPY ["Tours.Core/Tours.Core.csproj", "Tours.Core/"]
COPY ["Tours.Infrastructure/Tours.Infrastructure.csproj", "Tours.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Tours.API/Tours.API.csproj"

# Kopiranje ostatka source koda
COPY . .

# Build API projekta
WORKDIR "/src/Tours.API"
RUN dotnet build "Tours.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tours.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tours.API.dll"]
