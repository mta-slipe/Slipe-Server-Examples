#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SlipeTeamDeathmatch/SlipeTeamDeathmatch.csproj", "SlipeTeamDeathmatch/"]
RUN dotnet restore "SlipeTeamDeathmatch/SlipeTeamDeathmatch.csproj"
COPY . .
WORKDIR "/src/SlipeTeamDeathmatch"
RUN dotnet build "SlipeTeamDeathmatch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SlipeTeamDeathmatch.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 50666/udp
EXPOSE 50789/udp
EXPOSE 40680/tcp
EXPOSE 34219/udp
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SlipeTeamDeathmatch.dll"]