FROM microsoft/dotnet:2.2-sdk AS build-env

COPY . /FaceApp
RUN cd /FaceApp && \
    mkdir -p /dockerPackages && \
    dotnet publish *.csproj --output /dockerPackages

FROM mcr.microsoft.com/azure-functions/dotnet:2.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot

COPY --from=build-env ["dockerPackages", "/home/site/wwwroot"]