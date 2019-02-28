//Build script for Hot-Potato.NET
var HotPotatsln = "../HotPotato.sln";
var HotPotatoCoreTest = "../test/HotPotato.Core.Test/HotPotato.Core.Test.csproj";
var HotPotatoOpenApiTest = "../test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj";
var HotPotatoIntegrationTest = "../test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj";
var HotPotatoMiddlewareTest = "../test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj";
var HotPotatoE2ETest = "../test/HotPotato.E2E.Test/HotPotato.E2E.Test.csproj";

var target = Argument("target", "Default");

Task("NuGet-Restore")
	.Does(() => {
		DotNetCoreRestore(HotPotatsln);
	});

Task("Build")
	.Does(() => {
		DotNetCoreBuild(HotPotatsln, new DotNetCoreBuildSettings { NoRestore = true });
	});

Task("Run-Unit-Tests")
	.Does(() => {
		DotNetCoreTest(HotPotatoCoreTest, new DotNetCoreTestSettings {
			VSTestReportPath = "core-test-results.xml",
			NoRestore = true,
			NoBuild = true
		});
		DotNetCoreTest(HotPotatoMiddlewareTest, new DotNetCoreTestSettings {
			VSTestReportPath = "middleware-test-results.xml",
			NoRestore = true,
			NoBuild = true
		});
		DotNetCoreTest(HotPotatoOpenApiTest, new DotNetCoreTestSettings {
			VSTestReportPath = "openapi-test-results.xml",
			NoRestore = true,
			NoBuild = true
		});
	});
	
Task("Run-Integration-Tests")
	.Does(() => {
		DotNetCoreTest(HotPotatoIntegrationTest, new DotNetCoreTestSettings {
			VSTestReportPath = "integration-test-results.xml",
			NoRestore = true,
			NoBuild = true
		});
	});

Task("Run-E2E-Tests")
	.Does(() => {
		DotNetCoreTest(HotPotatpE2ETest, new DotNetCoreTestSettings {
			VSTestReportPath = "E2E-test-results.xml",
			NoRestore = true,
			NoBuild = true
		});
	});

Task("Default")
	.IsDependentOn("NuGet-Restore")
	.IsDependentOn("Build")
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Run-Integration-Tests")
	.IsDependentOn("Run-E2E-Tests");

RunTarget(target);
