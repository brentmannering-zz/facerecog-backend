FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env

COPY . /BlobApp

RUN cd /BlobApp && \
    mkdir -p /dockerPackages && \
    dotnet publish --configuration Release --output /dockerPackages

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /home/site/wwwroot

COPY --from=build-env /dockerPackages .

ENV RunFrom=RunFromDocker

CMD ["dotnet", "BlobApp.dll"]