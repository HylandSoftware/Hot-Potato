# Hot Potato Proxy

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

