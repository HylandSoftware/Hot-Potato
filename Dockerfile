FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
LABEL cache=true
ARG IMAGE_VERSION

RUN apt-get update && apt-get install -y curl sudo
RUN curl -fksSL https://certs.hyland.io/install.sh | sudo bash
RUN update-ca-certificates

WORKDIR /app
COPY . .

RUN dotnet build --configuration Release -p:Version=${IMAGE_VERSION} --output /app/build
RUN dotnet publish ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj --configuration Release -p:Version=${IMAGE_VERSION} --output /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 as runtime
LABEL maintainer "Test Automation Team <grp-automatedtesting@hyland.com>"

ENV SPEC_LOCATION "https://bitbucket.hylandqa.net/projects/AUTOTEST/repos/hot-potato/raw/test/RawPotatoSpec.yaml"
ENV REMOTE_ENDPOINT "http://localhost:9000"

COPY --from=build /app/publish /opt/hotpotato/

EXPOSE 3232
ENTRYPOINT dotnet /opt/hotpotato/HotPotato.AspNetCore.Host.dll --SpecLocation=${SPEC_LOCATION} --RemoteEndpoint=${REMOTE_ENDPOINT}
