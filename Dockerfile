FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
LABEL cache=true
ARG VERSION

RUN apt-get update && apt-get install -y curl sudo
RUN curl -fksSL https://certs.hyland.io/install.sh | sudo bash
RUN update-ca-certificates

WORKDIR /app
COPY . .

RUN dotnet build -c Docker -p:Version=${VERSION}
RUN dotnet publish -c Docker --framework=net5.0 --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime

LABEL maintainer "Test Automation Team <grp-automatedtesting@hyland.com>"

ENV SPEC_LOCATION "https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/raw/test/RawPotatoSpec.yaml"
ENV REMOTE_ENDPOINT "http://localhost:9000"

COPY --from=build /app/publish /opt/hotpotato/

EXPOSE 3232
ENTRYPOINT dotnet /opt/hotpotato/HotPotato.AspNetCore.Host.dll
