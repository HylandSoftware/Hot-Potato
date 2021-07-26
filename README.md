# Hot Potato Proxy

### Develop
[![Hot Potato CI](https://github.com/HylandSoftware/Hot-Potato/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/HylandSoftware/Hot-Potato/actions/workflows/ci.yml)
[![Coverage](http://shields.hyland.io/jenkins/c/https/autotest.jenkins.hylandqa.net/job/Prod%20Bitbucket/job/hot-potato/job/master.svg)](https://autotest.jenkins.hylandqa.net/job/Prod%20Bitbucket/job/hot-potato/job/master/lastSuccessfulBuild/cobertura/)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/derekadombek/a7688a704df34b13e2995c77340e1df6/raw/test.json)



The Hot Potato Proxy intends to function as an ASP.NET Core proxy that will validate an API's conformance to an OpenAPI spec.

## Setup

To use the complete tool you will need to download the `HotPotato.AspNetCore.Host` NuGet package from https://proget.onbase.net/feeds/NuGet/HotPotato.AspNetCore.Host/. Since Hot Potato is a dotnet global tool you can easily download it from Powershell or Command Prompt.

### Install
To install Hot Potato use the following command:
```sh
dotnet tool install -g hotpotato.aspnetcore.host --add-source https://proget.onbase.net/nuget/NuGet/
```
There are other options that can be utilized when downloading a dotnet tool. A complete list of options can be found here: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install

If the install is successful you will see a message like this:  
```sh
You can invoke the tool using the following command: HotPotato
Tool 'hotpotato.aspnetcore.host' (version '0.1.29') was successfully installed.
```
### Start Hot Potato

You can now start the tool by using the command `HotPotato`. Add the arguments for your testing situation and you can utilize `HotPotato` from the command line.
```sh
HotPotato --RemoteEndpoint http://hyland.io/my/api --SpecLocation http://hyland.io/my/spec.json
```

### Docker

Hot Potato can also be started in a Jenkins build through Docker. In a pipeline stage, you can create a network through Docker, pull our image from [Harbor](https://hcr.io/harbor/projects/118/repositories/automated-testing%2Fhot-potato/tags/latest), then start Hot Potato in that network.

A good example can be found in the [Jenkinsfile](https://bitbucket.hyland.com/projects/IBPA-CV/repos/cv-conformance-tests/browse/Jenkinsfile) for the CV Conformance Tests:

```groovy
stage('HotPotato'){			
	steps {
		sh 'docker network create hp'
		sh 'docker pull hcr.io/automated-testing/hot-potato:latest'
		sh 'docker run --rm -d --network hp --name Conformance -p 3232:3232 -e HttpClientSettings__IgnoreClientHttpsCertificateValidationErrors=true -e REMOTE_ENDPOINT=https://$Host/$ApiRoot -e SPEC_LOCATION=$ApiSpec hcr.io/automated-testing/hot-potato'                
        }			
	}
```

We ran into persisent SSL certificate validation issues when running in Docker, so setting the environment variable `HttpClientSettings__IgnoreClientHttpsCertificateValidationErrors` to `true` as shown above is necessary to run tests successfully.

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

### Accessing results via ResultCollector

When writing tests using a fixture to set up a mock server (such as TestServer), you can expose a public List<Result> member obtained from the IResultCollector service in the fixture to validate each Result instead of having to pull from the /results endpoint.

In the test fixture constructor:
```csharp
Results = hotPotatoServer.Host.Services.GetService<IResultCollector>().Results;
```

In test:
```csharp
Result result = results.ElementAt(0);

Assert.Equal(methodString, result.Method, ignoreCase: true);
Assert.Equal(pathUri.AbsolutePath, result.Path);
Assert.Equal(State.Pass, result.State);
Assert.Equal(expectedStatusCode, result.StatusCode);

results.Clear();
```

More information about writing tests using TestServer can be found below in the [Middleware](#middleware) section.

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

`hotpotato --RemoteEndpoint http://hyland.io/my/api --SpecLocation http://hyland.io/my/spec.json`

### HotPotato.Core

`HotPotato.Core` contains all of the models, HTTP logic, the proxy forwarder, and the interface for the proxy processor. This library is a dependency of `HotPotato.AspNetCore.Middleware` but will likely never need to be directly consumed.

### HotPotato.OpenApi

`HotPotato.OpenApi` contains all of the OpenAPI functionality including the processor implementation that validates an HTTP request/response pair against a spec, a result collection, logic to find paths, and so on. Currently, we are using the packages `NSwag`, `NJsonSchema`, and `NJsonSchema.Yaml` to consume a spec and to validate the contained schemas.

<a name="middleware"></a>
### HotPotato.AspNetCore.Middleware

This is an ASP.NET Core middleware that can be used in situations where test suites are directly starting up the server startup or using [`TestServer`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver).

The main factor in setting up a test suite to use the isolated middleware is that because Hot Potato's default behavior is to create its own client through DI setup, an instance of the API Under Test's client must be created first to be able to be injected into DI.

#### Using the Middleware with TestServer

In our example [test fixture](https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/browse/test/HotPotato.TestServer.Test/TestFixture.cs), we use two instances of TestServer: one to create the client representing the API Under Test, and one that consumes this client to create a new client housing the Hot Potato proxy.

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

//versions below 1.1.0
Core.Http.Default.HttpClient apiClient = new Core.Http.Default.HttpClient(apiServer.CreateClient());
//1.1.0+
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
//versions below 1.1.0
Client = new Core.Http.Default.HttpClient(hotPotatoServer.CreateClient());
//1.1.0+
Client = new HotPotatoClient(hotPotatoServer.CreateClient());
```

Then we use them like so in a test to send requests and verify the validation results:
```csharp
await client.SendAsync(req);

Result result = results.ElementAt(0);

Assert.Equal(State.Pass, result.State);
```

Make sure to call `results.Clear()` in a `Dispose()` method in XUnit or a `[Teardown]` method in NUnit. Another option is to call `results.Clear` in the `finally` block of a try-finally statement containing the test fixture. 

The full example test can be found at [RawPotatoTest.cs](https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/browse/test/HotPotato.TestServer.Test/RawPotatoTest.cs).

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

## Testing with Postman

End-to-End tests using Hot Potato can be run with Postman both locally and through a pipeline also using Newman.

To use Postman locally, you must have instances of both the Hot Potato server and your API server running.

For our test project, we provided our own sample Hot Potato API, which can be found [here](https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/browse/test/HotPotato.Api).

Once your System Under Test is ready, you may start writing Postman requests with the base address of localhost:3232.
To check the results of these requests, you can query the results endpoint as shown in the [Results](#results) section above.

You may also create these requests as part of a collection, which will allow for the creation of test sets, and the ability for them to be exported and run by a pipeline.

If you are not familiar with creating collections and writing tests in Postman, more information can be found in the links below:

[Collections](https://learning.getpostman.com/docs/postman/collections/creating-collections/)

[Tests](https://learning.getpostman.com/docs/postman/scripts/test_scripts/)

Tests will usually check for critical information such as if the correct status code and body are being returned correctly in the response.

Examples can be found in our HappyPath collection [here](https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/browse/test/HappyPathTests.postman_collection.json).

**Check that the response contains the correct status code and expected body**
```javascript
pm.test(\"LandingPage returns 200 OK\", function () {
	pm.response.to.have.status(200)
})

pm.test(\"LandingPage returns expected body \", function () {
	pm.response.to.have.body(\"https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/browse\")
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

### Running Postman tests in a in a pipeline
Once your collection and its tests are finished, you can export it as a json file. For our project, we exported our collections to the root \test folder. 

More information about exporting collections can be found [here](https://learning.getpostman.com/docs/postman/collections/data-formats/)

Now that the collection is in a directory, it can be used by the ```newman run``` command in Jenkins.
We do so in our "Run-E2E-Tests" stage in our [Jenkinsfile](https://bitbucket.hyland.com/projects/TATO/repos/hot-potato/browse/Jenkinsfile), where we start Hot Potato and our sample API.

```groovy
stage("Run-E2E-Tests") {
    steps {
        container("builder") {
            sh 'dotnet $WORKSPACE/src/HotPotato.AspNetCore.Host/bin/Release/netcoreapp3.1/HotPotato.AspNetCore.Host.dll &'
            sh 'dotnet $WORKSPACE/test/HotPotato.Api/bin/Release/netcoreapp3.1/HotPotato.Api.dll &'
        }
        container("newman") {
            sh 'newman run $WORKSPACE/test/HappyPathTests.postman_collection.json'
			sh 'newman run $WORKSPACE/test/Non-ConformantTests.postman_collection.json'
            sh 'newman run $WORKSPACE/test/NotInSpecTests.postman_collection.json'
        }
    }
}
```

## Known Issues

As of version 1.0.13, Hot Potato can handle multi-file specs, but unfortunately still cannot handle some edge cases involving schemas in a subdirectory having external references to other schemas in subdirectories. A [pull request](https://github.com/RicoSuter/NJsonSchema/pull/1356) has been submitted to [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) that would solve these edge cases.

A workaround for this issue is using the tool [swagger-merger](https://www.npmjs.com/package/swagger-merger) to combine multi-file specs into a single file.