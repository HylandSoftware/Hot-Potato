# Hot Potato Proxy 
### Develop
[![Build Status](https://autotest.jenkins.hylandqa.net/job/Test%20Automation%20Team/job/hot-potato-.net/job/develop/badge/icon)](https://autotest.jenkins.hylandqa.net/job/Test%20Automation%20Team/job/hot-potato-.net/job/develop/)

[![Coverage](http://shields.hyland.io/jenkins/c/https/autotest.jenkins.hylandqa.net/job/Test%20Automation%20Team/job/hot-potato-.net/job/develop.svg)](https://autotest.jenkins.hylandqa.net/job/Test%20Automation%20Team/job/hot-potato-.net/job/develop/lastSuccessfulBuild/cobertura/)

The Hot Potato Proxy intends to function as an ASP.NETCore proxy that will validate an API's conformance to an OpenAPI spec.

## Structure

The proxy is broken down into a number of components to allow flexibility for developers.

### HotPotato.AspNetCore.Host

This is an ASP.NETCore host configured to use the HotPotato Middleware. It is stood up as a separate server that listens by default on port `3232`. There is an `appsettings.json` to allow the developer to set the remote endpoing to forward requests to and the specification location of the OpenAPI spec to validate conformance. These values can also be passed into the command line via the following command

`hotpotato --RemoteEndpoint http://hyland.io/my/api --SpecLocation http://hyland.io/my/spec.json`

### HotPotato.AspNetCore.Middlware

This is an ASP.NETCore middleware that can be used in situations where test suites are directly starting up the server startup or using the `TestServer`. It can be consumed via the following methods:

```csharp

// Example code for TestServer

```

```csharp

// Example code for direct startup

```

### HotPotato.Core

`HotPotato.Core` contains all of the models, HTTP logic, the proxy forwarder, and the interface for the proxy processor. This library is a dependency of `HotPotato.AspNetCore.Middleware` but will likely never need to be directly consumed.

### HotPotato.OpenApi

`HotPotato.OpenApi` contains all of the OpenAPI functionality including the processor implementation that validates an HTTP request/response pair against a spec, a result collection, logic to find paths, and so on. Currently, we are using the packages `NSwag`, `NJsonSchema`, and `NJsonSchema.Yaml` to consume a spec and to validate the contained schemas.

## Setup

To use the complete tool you will need to download the `HotPotato.AspNetCore.Host` NuGet package from https://proget.onbase.net/feeds/NuGet/HotPotato.AspNetCore.Host/. Since `HotPotato` is a dotnet global tool you can easily download it from Powershell or Command Prompt.

### Install
To install `HotPotato` use the following command:
```sh
dotnet tool install -g hotpotato.aspnetcore.host --add-source https://proget.onbase.net/feeds/NuGet/HotPotato.AspNetCore.Host/
```
There are other options that can be utilized when downloading a dotnet tool. A complete list of options can be found here: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install

If the install is successful you will see a message like this:  
```sh
You can invoke the tool using the following command: HotPotato
Tool 'hotpotato.aspnetcore.host' (version '0.1.29') was successfully installed.
```
### Start HotPotato

You can now start the tool by using the command `HotPotato`. Add the arguments for your testing situation and you can utilize `HotPotato` from the command line.
```sh
hotpotato --RemoteEndpoint http://hyland.io/my/api --SpecLocation http://hyland.io/my/spec.json
```
