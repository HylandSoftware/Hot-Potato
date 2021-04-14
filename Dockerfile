FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
LABEL cache=true
ARG IMAGE_VERSION=1.0
ARG NET_FRAMEWORK=netcoreapp2.2
ENV VERSION $IMAGE_VERSION
ENV FRAMEWORK $NET_FRAMEWORK

RUN apt-get update && apt-get install -y curl sudo
RUN curl -fksSL https://certs.hyland.io/install.sh | sudo bash
RUN update-ca-certificates

WORKDIR /app
COPY . .

RUN dotnet build --configuration ExcludeTests --framework=${FRAMEWORK} -p:Version=${VERSION} --output /app/build
RUN dotnet publish ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj --framework=${FRAMEWORK} --configuration ExcludeTests -p:Version=${VERSION} --output /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 as runtime
#FROM mcr.microsoft.com/dotnet/aspnet:3.1

LABEL maintainer "Test Automation Team <grp-automatedtesting@hyland.com>"

ENV SPEC_LOCATION "https://bitbucket.hylandqa.net/projects/AUTOTEST/repos/hot-potato/raw/test/RawPotatoSpec.yaml"
ENV REMOTE_ENDPOINT "http://localhost:9000"

COPY --from=build /app/publish /opt/hotpotato/

EXPOSE 3232
ENTRYPOINT dotnet /opt/hotpotato/HotPotato.AspNetCore.Host.dll --SpecLocation=${SPEC_LOCATION} --RemoteEndpoint=${REMOTE_ENDPOINT}
