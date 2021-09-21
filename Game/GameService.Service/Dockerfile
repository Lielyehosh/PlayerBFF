FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Game/GameService/GameService.csproj", "GameService/"]
RUN dotnet restore "Game/GameService/GameService.csproj"
COPY . .
WORKDIR "/src/GameService"
RUN dotnet build "GameService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameService.dll"]
