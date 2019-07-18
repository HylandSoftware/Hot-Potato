FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
LABEL cache=true
ARG IMAGE_VERSION
WORKDIR /app
COPY . .
RUN dotnet build --configuration Release -p:Version=${IMAGE_VERSION} --output /app/build
RUN dotnet publish ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj --configuration Release -p:Version=${IMAGE_VERSION} --output /app/publish


FROM build AS test

WORKDIR /app
VOLUME [ "/app/test/results", "/app/test/coverage" ]
ENTRYPOINT dotnet test ./test/HotPotato.Core.Test/HotPotato.Core.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=/app/test/coverage/HotPotato.Core.Test.Coverage.xml -p:Exclude="[xunit.*]*" -l:"JUnit;LogFilePath=/app/test/results/HotPotato.Core.Test.Results.xml" && \
    dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=/app/test/coverage/HotPotato.Middleware.Test.Coverage.xml -p:Include="[*.Middleware]*" -l:"JUnit;LogFilePath=/app/test/results/HotPotato.Middleware.Test.Results.xml" && \
    dotnet test ./test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=/app/test/coverage/HotPotato.OpenAPI.Test.Coverage.xml -p:Include="[*.OpenApi]*" -l:"JUnit;LogFilePath=/app/test/results/HotPotato.OpenAPI.Test.Results.xml"

FROM build AS integrationtest

WORKDIR /app
VOLUME [ "/app/test/results" ]
ENTRYPOINT dotnet test ./test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj -c Release -l:"JUnit;LogFilePath=/app/test/results/integrationResults.xml"

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 as runtime
LABEL maintainer "Test Automation Team <grp-automatedtesting@hyland.com>"

ENV SPEC_LOCATION "https://bitbucket.hylandqa.net/projects/AUTOTEST/repos/hot-potato/raw/test/RawPotatoSpec.yaml"
ENV REMOTE_ENDPOINT "http://localhost:9000"

COPY --from=build /app/publish /opt/hotpotato/

EXPOSE 3232
ENTRYPOINT dotnet /opt/hotpotato/HotPotato.AspNetCore.Host.dll --SpecLocation=${SPEC_LOCATION} --RemoteEndpoint=${REMOTE_ENDPOINT}
