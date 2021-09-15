FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["AuthMS/AuthMS.csproj", "AuthMS/"]
RUN dotnet restore "AuthMS/AuthMS.csproj"
COPY . .
WORKDIR "/src/AuthMS"
RUN dotnet build "AuthMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthMS.dll"]
