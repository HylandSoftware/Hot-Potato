# Hot Potato Proxy

![Build Status](https://github.com/HylandSoftware/Hot-Potato/workflows/hot-potato-ci/badge.svg)

The Hot Potato is an ASP.NET Core reverse proxy that will validate an API's conformance to an OpenAPI spec.

## Setup

To use the complete tool you will need to download the `HotPotato.AspNetCore.Host` NuGet package from https://www.nuget.org/packages/HotPotato.AspNetCore.Host/. Since Hot Potato is a dotnet global tool you can easily download it from Powershell or Command Prompt. The most common way of utilizing this tool can be found in the [Postman](#postman) section below.

If you have an automated API testing project with a mock server you would like to conformance test, you also have the option of installing the `HotPotato.AspNetCore.Middleware` NuGet package from https://www.nuget.org/packages/HotPotato.AspNetCore.Middleware/. More information about writing conformance tests with a mock server can be found below in the [Middleware](#middleware) section.

### Install .NET Tool

To install Hot Potato use the following command:
```sh
dotnet tool install -g hotpotato.aspnetcore.host
```
There are other options that can be utilized when downloading a dotnet tool. A complete list of options can be found here: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install

If the install is successful you will see a message like this:  
```sh
You can invoke the tool using the following command: HotPotato
Tool 'hotpotato.aspnetcore.host' (version '2.0.0') was successfully installed.
```
### Start Hot Potato

You can now start the tool by using the command `HotPotato`. Add the arguments for your testing situation and you can utilize `HotPotato` from the command line.
```sh
HotPotato --RemoteEndpoint http://example.com/my/endpoint --SpecLocation http://example.com/my/specification.yaml
```

### SSL Validation Issues

We have also also provided an environment variable named `HttpClientSettings__IgnoreClientHttpsCertificateValidationErrors` that can be set to `true` in the case of of persistent SSL certificate validation issues. 

### Specs with Token Validation

In the case of something like accessing a raw file in a private repo on Github, a token is needed to access a specification. For cases like this, we have included a `SpecToken` environment variable that can be used as a secret. If you're running Hot Potato locally via Visual Studio, an easy way of setting the secret can be found here: [Manage User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows#json-structure-flattening-in-visual-studio).

<a name="results"></a>
## Results

In order to retrieve results from the proxy, we have exposed a `/results` endpoint. This endpoint will return a JSON-formatted object that shows all of the requests that have come through and whether or not they're conformant. The response will also include a `X-Status` header which will be either `Pass`, `Fail`, or `Inconclusive`.

__Pass Result__
```json
[
    {
        "Path":"/endpoint",
        "Method":"GET",
        "StatusCode":200,
        "State":"Pass"
    }
]
```

__Fail Result__
```json
[
    {        
        "Path":"/endpoint",
        "Method":"GET",
        "StatusCode":404,
        "State":"Fail",
        "Reasons":["InvalidBody"],
        "ValidationErrors":
        [
            {
                "Message":"Error",
                "Kind":"Unknown",
                "Property":"Property",
                "LineNumber":5,
                "LinePosition":10
            }
        ]
    }
]
```

### Custom Result Headers

Hot Potato also allows users to add custom headers to results objects. To do so, users can can add the prefix "X-HP-" to a header key in a request, and it will appear at the top of the result in a "custom" array. The custom array only appears if custom headers are provided.

```json
[
	{
        "custom": {
            "X-HP-Name": [
                "LandingPage"
            ]
        },
        "state": "Pass",
        "path": "/",
        "method": "get",
        "statusCode": 200
    }
]
```

## Structure

The proxy is broken down into a number of components to allow flexibility for developers.

### HotPotato.AspNetCore.Host

This is an ASP.NET Core host configured to use the Hot Potato Middleware. It is stood up as a separate server that listens by default on port `3232`. There is an `appsettings.json` to allow the developer to set the remote endpoint to forward requests to and the location of the OpenAPI specification to validate conformance. These values can also be passed into the command line via the following command:

`hotpotato --RemoteEndpoint http://example.com/my/endpoint --SpecLocation http://example.com/my/specification.yaml`

### HotPotato.Core

`HotPotato.Core` contains all of the models, HTTP logic, the proxy forwarder, and the interface for the proxy processor. This library is a dependency of `HotPotato.AspNetCore.Middleware` but will likely never need to be directly consumed.

### HotPotato.OpenApi

`HotPotato.OpenApi` contains all of the OpenAPI functionality including the processor implementation that validates an HTTP request/response pair against a spec, a result collection, logic to find paths, and so on. Currently, we are using the packages `NSwag`, `NJsonSchema`, and `NJsonSchema.Yaml` to consume a spec and to validate the contained schemas.

<a name="middleware"></a>
### HotPotato.AspNetCore.Middleware

This is an ASP.NET Core middleware that can be used in situations where test suites are directly starting up the server startup or using [`TestServer`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver).

The main factor in setting up a test suite to use the isolated middleware is that because Hot Potato's default behavior is to create its own client through DI setup, an instance of the API Under Test's client must be created first to be able to be injected into DI.

#### Using the Middleware with TestServer

In our example [test fixture](https://github.com/HylandSoftware/Hot-Potato/blob/master/test/HotPotato.TestServer.Test/TestFixture.cs), we use two instances of TestServer: one to create the client representing the API Under Test, and one that consumes this client to create a new client housing the Hot Potato proxy.

First, we define a TestFixture class to use a generic Startup reference type:
 ```csharp
public class TestFixture<TStartup> : IDisposable where TStartup : class
 ```

Next, TestServer doesn't actually listen on an address, but it needs a placeholder BaseAddress to be used by the HttpRequest constructors. In our example, we use localhost:5000 for the TestServer housing the API and localhost:3232 for the TestServer housing the Hot Potato proxy to be consistent with the rest of our testing. These can be set to anything as long as they are valid URIs.

```
private const string ApiServerAddress = "http://localhost:5000";
private const string HotPotatoAddress = "http://localhost:3232";
```

Then we create a TestServer instance using the Startup type to create a client:
```csharp
var apiBuilder = new WebHostBuilder()
    .UseStartup<TStartup>();

apiServer = new TestServer(apiBuilder);
apiServer.BaseAddress = new Uri(TestServerAddress);

HotPotatoClient apiClient = new HotPotatoClient(apiServer.CreateClient());
```

Now that we have a client created for the API Under Test, we can build our web host and inject it into the Test Server.
This is done in our fixture setup, but can also be placed in a custom Startup class:

```csharp
var hotPotatoBuilder = new WebHostBuilder()
    //Setting this here instead of in appsettings.json so it always matches the BaseAddress on TestServer
    .UseSetting("RemoteEndpoint", TestServerAddress)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true);
    })
    .ConfigureServices(services =>
    {
        services.ConfigureMiddlewareServices(apiClient);
    })
    .Configure(builder =>
    {
        builder.UseMiddleware<HotPotatoMiddleware>();
    });

    hotPotatoServer = new TestServer(hotPotatoBuilder);
    hotPotatoServer.BaseAddress = new Uri(HotPotatoAddress);
```

An important part of this builder is the line:
```csharp
services.ConfigureMiddlewareServices(apiClient);
```
`ConfigureMiddlewareServices` is an extension method found in the AspNetCore.Middleware project that adds the client to DI as well as all the services necessary to use the Middleware.

These services are `IProxy`, `ISpecificationProivder`, `IResultCollector`, and `IProcessor`.

We set the fixture's public members of Results to the List<Result> member of the IResultCollector and Client to the client created with the Hot Potato TestServer:
```csharp
Results = hotPotatoServer.Host.Services.GetService<IResultCollector>().Results;
Client = new HotPotatoClient(hotPotatoServer.CreateClient());
```

Then we use them like so in a test to send requests and verify the validation results:
```csharp
await client.SendAsync(req);

Result result = results.ElementAt(0);

Assert.Equal(State.Pass, result.State);
```

Make sure to call `results.Clear()` in a `Dispose()` method in XUnit or a `[Teardown]` method in NUnit. Another option is to call `results.Clear` in the `finally` block of a try-finally statement containing the test fixture. 

The full example test can be found at [RawPotatoTest.cs](https://github.com/HylandSoftware/Hot-Potato/blob/master/test/HotPotato.TestServer.Test/RawPotatoTest.cs).

#### Using Middleware/TestServer with Startups in separate test projects

As mentioned above, the fixture setup can be done in a Startup class, and to avoid re-writing the same code in multiple test projects, the Startup class from one test project can be referenced by another. When choosing this route, the test Startup class will need to be registered as a MVC Application Part with the line `services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);`.

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    startup.ConfigureServices(services);

    services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);

    //custom code here
}
```

<a name="postman"></a>
## Testing with Postman

End-to-End tests using Hot Potato can be run with Postman both locally and through a pipeline also using Newman.

To use Postman locally, you must have instances of both the Hot Potato server and your API server running.

For our test project, we provided our own sample Hot Potato API, which can be found [here](https://github.com/HylandSoftware/Hot-Potato/tree/master/test/HotPotato.Api).

Once your System Under Test is ready, you may start writing Postman requests with the base address of localhost:3232.
To check the results of these requests, you can query the results endpoint as shown in the [Results](#results) section above.

You may also create these requests as part of a collection, which will allow for the creation of test sets, and the ability for them to be exported and run by a pipeline.

If you are not familiar with creating collections and writing tests in Postman, more information can be found in the links below:

[Collections](https://learning.getpostman.com/docs/postman/collections/creating-collections/)

[Tests](https://learning.getpostman.com/docs/postman/scripts/test_scripts/)

Tests will usually check for critical information such as if the correct status code and body are being returned correctly in the response.

Examples can be found in our HappyPath collection [here](https://github.com/HylandSoftware/Hot-Potato/blob/master/test/HappyPathTests.postman_collection.json).

**Check that the response contains the correct status code and expected body**
```javascript
pm.test(\"LandingPage returns 200 OK\", function () {
	pm.response.to.have.status(200)
})

pm.test(\"LandingPage returns expected body \", function () {
	pm.response.to.have.body(\"https://github.com/HylandSoftware/Hot-Potato\")
})
```

At the end of each collection, a GET request can be sent to the /results endpoint so that the list of results can be tested.
We split our test suite into three different collections of HappyPath, Non-Conformant, and NotInSpec tests, so that we could easily check if each collection only contained either all 'Pass' or all 'Fail' results.

**Check that the HappyPath results do not contain fail results**
```javascript
pm.test(\"Results should not contain Fail\", function () {
	pm.expect(pm.response.text()).to.not.include(\"Fail\")
})
```

Make sure to send a DELETE request at the end of your collection so that the results from collection do not carry over to the next.
