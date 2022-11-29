FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENV STOP_RATIO=1.3
ENV HELP_SOLO_PEERS=true
ENV TRANSMISSION_PATH=""
ENTRYPOINT ["dotnet", "transmission-watcher.dll"]