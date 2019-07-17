FROM hcr.io/library/dotnet:2.1-sdk-docker AS build
LABEL cache=true
ARG IMAGE_VERSION
WORKDIR /app
COPY . .
RUN dotnet build --configuration Release -p:Version=${IMAGE_VERSION} -o ./build
RUN dotnet publish ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj --configuration Release -p:Version=${IMAGE_VERSION} --output /app/publish

FROM build AS test

WORKDIR /app

ENTRYPOINT dotnet test ./test/HotPotato.Core.Test/HotPotato.Core.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/coreCoverage.xml -p:Exclude="[xunit.*]*" -l:"JUnit;LogFilePath=./test/results/coreResults.xml" && \
    dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/middlewareCoverage.xml -p:Include="[*.Middleware]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/middlewareResults.xml" && \
    dotnet test ./test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/openApiCoverage.xml -p:Include="[*.OpenApi]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/openapiResults.xml"

FROM microsoft/dotnet:2.1-aspnetcore-runtime as runtime
LABEL maintainer "Test Automation Team <grp-automatedtesting@hyland.com>"

ENV SPEC_LOCATION "https://bitbucket.hylandqa.net/projects/AUTOTEST/repos/hot-potato/raw/test/RawPotatoSpec.yaml"
ENV REMOTE_ENDPOINT "http://localhost:9000"

COPY --from=build /app/publish /opt/hotpotato/

EXPOSE 3232
ENTRYPOINT dotnet /opt/hotpotato/HotPotato.AspNetCore.Host.dll --SpecLocation=${SPEC_LOCATION} --RemoteEndpoint=${REMOTE_ENDPOINT}
