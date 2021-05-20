FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
RUN groupadd --gid 1000 ubuntu && useradd --uid 1000 --gid ubuntu --shell /bin/bash --create-home ubuntu
RUN apt-get update -qq
RUN apt-get install unrar -qq
COPY bin/Release/net5.0/publish/ App/
COPY config.json App/config.json
WORKDIR /App
ENTRYPOINT ["dotnet", "transmission-watcher.dll"]
USER ubuntu