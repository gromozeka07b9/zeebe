FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StateChanger.csproj", "./"]
RUN dotnet restore "StateChanger.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "StateChanger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StateChanger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StateChanger.dll"]
