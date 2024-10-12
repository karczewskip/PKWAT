FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PKWAT.ScoringPoker.Server/PKWAT.ScoringPoker.Server.csproj", "PKWAT.ScoringPoker.Server/"]
COPY ["PKWAT.ScoringPoker.Contracts/PKWAT.ScoringPoker.Contracts.csproj", "PKWAT.ScoringPoker.Contracts/"]
COPY ["PKWAT.ScoringPoker.Domain/PKWAT.ScoringPoker.Domain.csproj", "PKWAT.ScoringPoker.Domain/"]
RUN dotnet restore "./PKWAT.ScoringPoker.Server/PKWAT.ScoringPoker.Server.csproj"
COPY . .
WORKDIR "/src/PKWAT.ScoringPoker.Server"
RUN dotnet build "./PKWAT.ScoringPoker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PKWAT.ScoringPoker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PKWAT.ScoringPoker.Server.dll"]