//Build script for Hot-Potato.NET
var HotPotatsln = "../HotPotato.sln";
var HotPotatoTest = "../test/HotPotato.Test/HotPotato.Test.csproj";
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

Task("Run-Unit-Tests")
	.Does(() => {
		DotNetCoreTest(HotPotatoTest, new DotNetCoreTestSettings {
			VSTestReportPath = "unit-test-results.xml",
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
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Run-Integration-Tests");

RunTarget(target);
