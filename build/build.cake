//Build script for Hot-Potato.NET
var HotPotatsln = "../HotPotato.sln";
var HotPotatoOpenApiTest = "../test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj";
var HotPotatoIntegrationTest = "../test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj";

var target = Argument("target", "Default");

Task("NuGet-Restore")
	.Does(() => {
		DotNetCoreRestore(HotPotatsln);
	});

Task("Build")
	.Does(() => {
		DotNetCoreBuild(HotPotatsln, new DotNetCoreBuildSettings { NoRestore = true });
	});

Task("Run-OpenApi-Tests")
	.Does(() => {
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

Task("Default")
	.IsDependentOn("NuGet-Restore")
	.IsDependentOn("Build")
	.IsDependentOn("Run-OpenApi-Tests")
	.IsDependentOn("Run-Integration-Tests");

RunTarget(target);
